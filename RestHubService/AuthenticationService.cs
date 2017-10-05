using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestHubService
{
    public class AuthenticationService
    {
        public bool loginUser(String username, String password)
        {
            try
            {
                using (RestaurantHubEntities entities = new RestaurantHubEntities())
                {
                    bool isValidUser = entities.customers.Where(x => x.email.Equals(username) && x.password.Equals(password)).Count() > 0;

                    return isValidUser;
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle Exceptions properly. Setup email exception to developer service.
                return false;
            }
        }
        
        public int registerUser(String email, String password, String name, String phone)
        {
            try
            {
                using (RestaurantHubEntities entities = new RestaurantHubEntities())
                {
                    customer c = new customer();
                    c.email = email;
                    c.password = password;
                    c.name = name;
                    c.phone = phone;
                    c.account_creation_date = DateTime.Now;

                    entities.customers.Add(c);

                    entities.SaveChanges();

                    return c.customer_id;
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle Exceptions properly. Setup email exception to developer service.
                return 0;
            }
        }

        public bool addCreditCard(String nameOnCard, String creditCardNumber, String cvv, int expMonth, int expYear, int customerid, String addrLine1, String addrLine2, String city, String state, String postCode, String country)
        {
            try
            {
                using (RestaurantHubEntities entities = new RestaurantHubEntities())
                {
                    // Adding Billing Address

                    customer_addresses ca = new customer_addresses();
                    ca.customer_id = customerid;
                    ca.street_address = addrLine2.Equals("") ? addrLine1 : addrLine1 + ", " + addrLine2;
                    ca.city = city;
                    ca.state = state;
                    ca.zip_code = postCode;
                    ca.country = country;

                    entities.customer_addresses.Add(ca);
                    entities.SaveChanges();

                    // Adding Credit Card

                    customer_credit_card cc = new customer_credit_card();
                    cc.customer_id = customerid;
                    cc.cc_name = nameOnCard;

                    // Encrypting Credit Card number using guid.
                    Guid guid = Guid.NewGuid();
                    String cypherCreditCard = StringCipher.Encrypt(creditCardNumber, guid.ToString());
                    cc.cc_number = cypherCreditCard;

                    cc.cc_expiration = new DateTime(expYear, expMonth, DateTime.DaysInMonth(expYear, expMonth)); // Setting expiration to the last day of the month and year specified.

                    cc.cc_type = "TODO"; // TODO: Dhara - We can write a code to infer Type from first 4 digits of the card.
                    cc.cvv_number = cvv;
                    cc.billing_address_id = ca.address_id;

                    entities.customer_credit_card.Add(cc);
                    entities.SaveChanges();

                    // Adding Guid as Key in CC_Encryption
                    cc_encryption cce = new cc_encryption();
                    cce.credit_card_id = cc.credit_card_id;
                    cce.encryption_key = guid.ToString();

                    entities.cc_encryption.Add(cce);
                    entities.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle Exceptions properly. Setup email exception to developer service.
                return false;
            }
            return false;
        }

    //    public int addOrder(DateTime order_date_time, DateTime order_pickup_time, string order_status, decimal subtotal, decimal tax, decimal discount, decimal dicount, decimal total_charged )
    //    {
    //        try
    //        {
    //            using (RestaurantHubEntities entities = new RestaurantHubEntities())
    //            {
    //                restaurant_orders ro = new restaurant_orders();
    //                ro.order_date_time = DateTime.Now;
    //                ro.pickup_date_time = DateTime.Now;
    //                ro.subtotal = subtotal;
    //                ro.tax = tax;
    //                ro.discount = discount;
    //                ro.total_charged = total_charged;
                     

    //                entities.restaurant_orders.Add(ro);

    //                entities.SaveChanges();

    //                return ro.order_id;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // TODO: Handle Exceptions properly. Setup email exception to developer service.
    //            return 0;
    //        }
    //    }
    }

    public static class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
