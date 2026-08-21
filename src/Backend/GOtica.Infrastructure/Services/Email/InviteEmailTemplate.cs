using MimeKit;
using System.Net;

namespace GOtica.Infrastructure.Services.Email;

internal static class InviteEmailTemplate
{
    public static MimeEntity Generate(string ownerName, string inviteLink)
    {
        var encodedOwnerName =
            WebUtility.HtmlEncode(ownerName);

        var textBody = $"""
            You have been invited to G-Otica.

            {ownerName} invited you to join their optical store.

            Access the link below to accept your invitation:

            {inviteLink}
            """;

        var htmlBody = """
            <html>
            <head>
                <style>
                    body {
                        margin: 0;
                        background-color: #f4f6f8;
                        font-family: Arial, sans-serif;
                    }

                    .card {
                        background-color: #ffffff;
                        border: 2px solid #d1d9e0;
                        border-radius: 12px;
                        padding: 60px 50px;
                        max-width: 500px;
                        margin: 40px auto;
                        text-align: center;
                    }

                    h1 {
                        font-size: 28px;
                        color: #1a1a1a;
                    }

                    p {
                        font-size: 16px;
                        color: #4a4a4a;
                    }

                    .accept-button {
                        display: inline-block;
                        padding: 12px 28px;
                        background-color: #ffffff;
                        color: #1a1a1a;
                        border: 2px solid #1a1a1a;
                        border-radius: 8px;
                        text-decoration: none;
                        font-weight: 600;
                    }
                </style>
            </head>

            <body>
                <div class="card">
                    <h1>You have been invited!</h1>

                    <p>
                        You received an invitation to join
                        <strong>__OWNER_NAME__'s optical store</strong>.
                    </p>

                    <a href="__LINK__" class="accept-button">
                        Accept invitation
                    </a>
                </div>
            </body>
            </html>
            """
            .Replace("__OWNER_NAME__", encodedOwnerName)
            .Replace("__LINK__", inviteLink);

        var body = new BodyBuilder
        {
            TextBody = textBody,
            HtmlBody = htmlBody
        };

        return body.ToMessageBody();
    }
}
