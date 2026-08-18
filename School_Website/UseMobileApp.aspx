<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UseMobileApp.aspx.cs" Inherits="UseMobileApp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RITeSchool - Switch to Mobile App</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #1a237e 0%, #0d47a1 50%, #01579b 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
        }

        .container {
            background: #ffffff;
            border-radius: 16px;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
            max-width: 680px;
            width: 100%;
            padding: 48px 40px;
            animation: fadeInUp 0.6s ease-out;
        }

        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .header {
            text-align: center;
            margin-bottom: 32px;
        }

        .header .icon {
            font-size: 48px;
            margin-bottom: 12px;
        }

        .header h1 {
            color: #1a237e;
            font-size: 24px;
            font-weight: 700;
            margin-bottom: 4px;
        }

        .header .subtitle {
            color: #546e7a;
            font-size: 14px;
            font-weight: 400;
        }

        .content {
            color: #37474f;
            font-size: 15px;
            line-height: 1.7;
        }

        .content p {
            margin-bottom: 16px;
        }

        .content .highlight-date {
            color: #c62828;
            font-weight: 700;
        }

        .content .app-name {
            font-weight: 700;
            color: #1a237e;
        }

        .steps-section {
            background: #f5f7fa;
            border-radius: 12px;
            padding: 24px;
            margin: 24px 0;
        }

        .steps-section h3 {
            color: #1a237e;
            font-size: 16px;
            font-weight: 700;
            margin-bottom: 16px;
        }

        .step {
            display: flex;
            align-items: flex-start;
            margin-bottom: 16px;
        }

        .step:last-child {
            margin-bottom: 0;
        }

        .step-number {
            background: #1a237e;
            color: #ffffff;
            width: 28px;
            height: 28px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 13px;
            font-weight: 700;
            flex-shrink: 0;
            margin-right: 12px;
            margin-top: 2px;
        }

        .step-content {
            flex: 1;
        }

        .step-content .step-title {
            font-weight: 700;
            color: #263238;
            margin-bottom: 4px;
        }

        .step-content .step-desc {
            color: #546e7a;
            font-size: 14px;
        }

        .store-links {
            display: flex;
            gap: 12px;
            margin-top: 10px;
            flex-wrap: wrap;
        }

        .store-link {
            display: inline-flex;
            align-items: center;
            padding: 10px 20px;
            border-radius: 8px;
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
            transition: transform 0.2s, box-shadow 0.2s;
        }

        .store-link:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        }

        .store-link.google-play {
            background: #01875f;
            color: #ffffff;
        }

        .store-link.app-store {
            background: #000000;
            color: #ffffff;
        }

        .store-link .store-icon {
            margin-right: 8px;
            font-size: 18px;
        }

        .store-link .store-svg {
            margin-right: 8px;
            flex-shrink: 0;
        }
        
        .support-section {
            background: #fff8e1;
            border-left: 4px solid #f9a825;
            border-radius: 8px;
            padding: 16px 20px;
            margin: 24px 0;
            font-size: 14px;
            color: #37474f;
            line-height: 1.6;
        }

        .support-section a {
            color: #0d47a1;
            font-weight: 600;
            text-decoration: none;
        }

        .support-section a:hover {
            text-decoration: underline;
        }

        .footer-note {
            text-align: center;
            color: #546e7a;
            font-size: 14px;
            margin-top: 24px;
            padding-top: 20px;
            border-top: 1px solid #eceff1;
        }

        /* Responsive */
        @media (max-width: 600px) {
            .container {
                padding: 32px 24px;
            }

            .header h1 {
                font-size: 20px;
            }

            .content {
                font-size: 14px;
            }

            .store-links {
                flex-direction: column;
            }

            .store-link {
                justify-content: center;
            }

            .steps-section {
                padding: 18px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <div class="icon">&#128241;</div>
                <h1>Switch to the RITeSchool Mobile App</h1>
                <div class="subtitle">A better experience awaits you</div>
            </div>

            <div class="content">
                <p>
                    We are excited to announce that the <span class="app-name">RITeSchool Mobile App</span> now features all the tools, updates, and student information previously accessible through our website.
                </p>
                <p>
                    To provide you with a faster, modern, and seamless experience, we are retiring our web application on or around <span class="highlight-date">August 17, 2026</span>. After this date, all school-related updates and operations can be managed exclusively through the mobile app only.
                </p>
            </div>

            <div class="steps-section">
                <h3>How to switch if you are not using the mobile app yet:</h3>

                <div class="step">
                    <div class="step-number">1</div>
                    <div class="step-content">
                        <div class="step-title">Download the App</div>
                        <div class="step-desc">Search for "RITeSchool" on the Google Play Store or Apple App Store.</div>
                        <div class="store-links">

                        <a href="https://play.google.com/store/apps/details?id=www.riteschool.net" target="_blank" class="store-link google-play">
                                <svg class="store-svg" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="white"><path d="M3.609 1.814L13.792 12 3.61 22.186a.996.996 0 0 1-.61-.92V2.734a1 1 0 0 1 .609-.92zm10.89 10.893l2.302 2.302-10.937 6.333 8.635-8.635zm3.199-3.198l2.807 1.626a1 1 0 0 1 0 1.73l-2.808 1.626L15.206 12l2.492-2.491zM5.864 2.658L16.8 8.99l-2.302 2.302-8.634-8.634z"/></svg>
                                Google Play Store
                            </a>
                            <a href="https://apps.apple.com/in/app/riteschool/id1036759360" target="_blank" class="store-link app-store">
                                <svg class="store-svg" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="white"><path d="M18.71 19.5c-.83 1.24-1.71 2.45-3.05 2.47-1.34.03-1.77-.79-3.29-.79-1.53 0-2 .77-3.27.82-1.31.05-2.3-1.32-3.14-2.53C4.25 17 2.94 12.45 4.7 9.39c.87-1.52 2.43-2.48 4.12-2.51 1.28-.02 2.5.87 3.29.87.78 0 2.26-1.07 3.8-.91.65.03 2.47.26 3.64 1.98-.09.06-2.17 1.28-2.15 3.81.03 3.02 2.65 4.03 2.68 4.04-.03.07-.42 1.44-1.38 2.83M13 3.5c.73-.83 1.94-1.46 2.94-1.5.13 1.17-.34 2.35-1.04 3.19-.69.85-1.83 1.51-2.95 1.42-.15-1.15.41-2.35 1.05-3.11z"/></svg>
                                Apple App Store
                            </a>

                        </div>
                    </div>
                </div>

                <div class="step">
                    <div class="step-number">2</div>
                    <div class="step-content">
                        <div class="step-title">Log In</div>
                        <div class="step-desc">Use your existing Web App login credentials.</div>
                    </div>
                </div>
            </div>

            <div class="support-section">
                If you face issues downloading or logging in, please contact our <strong>Support Team</strong> at 
                <a href="mailto:schoolsupport@regulusit.net">schoolsupport@regulusit.net</a>.                
            </div>

            <div class="footer-note">
                Thank you for your support as we upgrade our systems to serve you better.
            </div>
        </div>
    </form>
</body>
</html>
