using Amazon;
using Amazon.CognitoIdentity;

namespace Networks
{
    public class CredentialHolder
    {
        public CognitoAWSCredentials Credentials { get; }
        
        public CredentialHolder()
        {
            Credentials = new CognitoAWSCredentials(
                NetworkSetting.IdPool,
                NetworkSetting.Region
            );
        }
    }
}