namespace piedteam_net1_2_hocmienphi.service.Utils.Mail;

public class MailContent
{
    public required string To { get; set; } // dia chi gui den
    public required string Subject { get; set; } // chu de (tieu de mail)
    public required string Body { get; set; } // noi dung (ho tro HTML) cua mail
}