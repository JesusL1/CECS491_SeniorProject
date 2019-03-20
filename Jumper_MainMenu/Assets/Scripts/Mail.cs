using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Collections;


public class Mail : MonoBehaviour
{
    public InputField yourEmail;
    public InputField subject;
    public InputField message;
    public Text error;

    public void sendmail_start()
    {
        StartCoroutine(sendmail());
    }

    public IEnumerator sendmail()
    {
        yield return new WaitForSeconds(0.0f);
        if (message.text == "" || subject.text == "" || yourEmail.text == "")
            error.text = "Please fill in all the fields";
        else
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(yourEmail.text);
            mail.To.Add("theJumpGame@gmail.com");
            mail.Subject = subject.text;
            mail.Body = "Mail from : " + yourEmail.text + "\n --------------------- \n \n" + message.text;

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.EnableSsl = true;
	    smtpServer.UseDefaultCredentials = false;
            smtpServer.Credentials = new System.Net.NetworkCredential("theJumpGame@gmail.com", "jumperXD123") as ICredentialsByHost;

            smtpServer.Send(mail);
            error.text = "Message has been sent successfully.";

        }


    }
}
