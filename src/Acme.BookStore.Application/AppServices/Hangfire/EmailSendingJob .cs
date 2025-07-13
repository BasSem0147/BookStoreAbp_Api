using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace Acme.BookStore.AppServices.Hangfire
{
    [Queue("alpha")]
    public class EmailSendingJob : BackgroundJob<EmailSendingArgs>, ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        public EmailSendingJob(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public  override  async void Execute(EmailSendingArgs args)
        {
           await _emailSender.SendAsync(
                args.EmailAddress,
                args.Subject,
                args.Body
            );
        }
        public  async Task ExecuteAsync()
        {
            EmailSendingArgs args =new EmailSendingArgs() { Body="Hello",EmailAddress="swebasem@gmail.com",Subject="Send"};
            await _emailSender.SendAsync(
                 args.EmailAddress,
                 args.Subject,
                 args.Body
             );
        }
    }
    //[Queue("alpha")]
    //public class HangFireJob
    //    : AsyncBackgroundJob<EmailSendingArgs>, ITransientDependency
    //{
    //    private readonly IEmailSender _emailSender;

    //    public HangFireJob(IEmailSender emailSender)
    //    {
    //        _emailSender = emailSender;
    //    }

    //    public override async Task ExecuteAsync(EmailSendingArgs args)
    //    {
    //        await _emailSender.SendAsync(
    //            args.EmailAddress,
    //            args.Subject,
    //            args.Body
    //        );
    //    }
    //}
    public class EmailSendingArgs
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
