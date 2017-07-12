using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vaConnect
{
    public class WiFiProfile
    {

        private String token;
        private String os;
        private UserPolicies user_policies;
        private String user_identity;

        public WiFiProfile()
        {
        }

        public WiFiProfile(String token, String os, UserPolicies user_policies, String user_identity)
        {
            this.token = token;
            this.os = os;
            this.user_policies = user_policies;
            this.user_identity = user_identity;
        }

        public String getToken()
        {
            return token;
        }

        public void setToken(String token)
        {
            this.token = token;
        }

        public String getOs()
        {
            return os;
        }

        public void setOs(String os)
        {
            this.os = os;
        }

        public UserPolicies getUser_policies()
        {
            return user_policies;
        }

        public void setUser_policies(UserPolicies user_policies)
        {
            this.user_policies = user_policies;
        }

        public String getUser_identity()
        {
            return user_identity;
        }

        public void setUser_identity(String user_identity)
        {
            this.user_identity = user_identity;
        }



    }
    public class UserPolicies
    {

        private String auth_type;
        private String eap_type;
        private String ssid;
        private String username;
        private String password;
        private String profile_name;
        private bool auto_connect;
        private String public_ca;
        private String user_cert;
        private String private_cert;
        private String private_cert_pass;

        public UserPolicies()
        {
        }

        public UserPolicies(String auth_type, String eap_type, String ssid, String username, String password, String profile_name, bool auto_connect, String public_ca, String user_cert, String private_cert, String private_cert_pass)
        {
            this.auth_type = auth_type;
            this.eap_type = eap_type;
            this.ssid = ssid;
            this.username = username;
            this.password = password;
            this.profile_name = profile_name;
            this.auto_connect = auto_connect;
            this.public_ca = public_ca;
            this.user_cert = user_cert;
            this.private_cert = private_cert;
            this.private_cert_pass = private_cert_pass;
        }

        public String getAuth_type()
        {
            return auth_type;
        }

        public void setAuth_type(String auth_type)
        {
            this.auth_type = auth_type;
        }

        public String getEap_type()
        {
            return eap_type;
        }

        public void setEap_type(String eap_type)
        {
            this.eap_type = eap_type;
        }

        public String getSsid()
        {
            return ssid;
        }

        public void setSsid(String ssid)
        {
            this.ssid = ssid;
        }

        public String getUsername()
        {
            return username;
        }

        public void setUsername(String username)
        {
            this.username = username;
        }

        public String getPassword()
        {
            return password;
        }

        public void setPassword(String password)
        {
            this.password = password;
        }

        public String getProfile_name()
        {
            return profile_name;
        }

        public void setProfile_name(String profile_name)
        {
            this.profile_name = profile_name;
        }

        public bool isAuto_connect()
        {
            return auto_connect;
        }

        public void setAuto_connect(bool auto_connect)
        {
            this.auto_connect = auto_connect;
        }

        public String getPublic_ca()
        {
            return public_ca;
        }

        public void setPublic_ca(String public_ca)
        {
            this.public_ca = public_ca;
        }

        public String getUser_cert()
        {
            return user_cert;
        }

        public void setUser_cert(String user_cert)
        {
            this.user_cert = user_cert;
        }

        public String getPrivate_cert()
        {
            return private_cert;
        }

        public void setPrivate_cert(String private_cert)
        {
            this.private_cert = private_cert;
        }

        public String getPrivate_cert_pass()
        {
            return private_cert_pass;
        }

        public void setPrivate_cert_pass(String private_cert_pass)
        {
            this.private_cert_pass = private_cert_pass;
        }
    }

}
