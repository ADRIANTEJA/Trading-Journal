using Prism.Events;

namespace API.Events;
/// <summary>
/// event raised when an Account is created in the MainModule AccountViewModel class
/// it takes a boolean parameter to notify weather the account was created succesfully
/// or not
/// </summary>
public class CreateAccountEvent : PubSubEvent<bool>
{

}
