using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptSharp;

namespace BLL
{
    public class Authentication
    {
        IDBHandler handler = new DBHandler();

        Functions function = new Functions();

        public bool checkForAccount(Model.Profile profileToCheck, bool register)
        {
            bool exists = false;
            //check if the account exists and it is a emmail count type
            try
            {
                Model.Profile profile = handler.getProfile(profileToCheck);

                if (profile == null)
                {
                    //the use accoun dose not exist
                }
                else if (profile != null)
                {
                    exists = true;
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in checkForAccount method of Authentication");
                throw new ApplicationException(e.ToString() +
                    "Error in checkForAccount method of Authentication");
            }
            return exists;
        }

        public string[] AuthenticateEmail(Model.Profile profileToCheck, string password)
        {
            //array data 0 = ID, 1 = User Type, 2 = Name
            string[] UserCookieDetails = { "Error", "" };

            //check if the account credentials are correct
            try
            {
                Model.Profile profile = handler.getProfile(profileToCheck);

                UserCookieDetails[1] = "N/A";

                if (profile != null)
                {
                    if (verifyPass(
                        password,
                        profile.Password
                        ) == true)
                    {
                            UserCookieDetails[0] = profile.ProfileID.ToString();
                            UserCookieDetails[1] = profile.FirstName.ToString() + " " + profile.LastName.ToString();
                    }
                    else
                    {
                        UserCookieDetails[0] = "PassN/A";
                    }
                }
                else
                {
                    UserCookieDetails[0] = "AccountN/A";
                }
            }
            catch (Exception e)
            {
                function.logAnError(e.ToString() +
                    "Error in checkForAccountEmail method of Authentication");
                throw new ApplicationException(e.ToString() +
                    "Error in checkForAccountEmail method of Authentication");
            }
            return UserCookieDetails;
        }

        public bool NewUser(Model.Profile user)
        {
            //return false if User creation is a failure
            bool succes = true;

            //creat new User
            try
            {
                try
                {
                    user.Password = generatePassHash(user.Password);
                    handler.newprofile(user);
                }
                catch (ApplicationException e)
                {
                    function.logAnError("Faild to add new acoount NewUser method of BLL.Authentication class" + e);
                    throw new ApplicationException(". We are unable to create a new accounmt at this time try again later.");
                }
            }
            catch
            {
                succes = false;
            }

            //return results
            return succes;
        }

        public string generatePassHash(string password)
        {
            return Crypter.Blowfish.Crypt(password);
        }

        private bool verifyPass(string password, string hash)
        {
            return Crypter.CheckPassword(password, hash);
        }

        public string generatePassRestCode()
        {
            string result;
            int[] id = new int[9];
            Random rn = new Random();
            for (int i = 0; i < id.Length; i++)
            {
                id[i] = rn.Next(0, 9);
            }
            result = string.Join("", id);
            return result;
        }
    }
}
