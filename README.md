# Clipboard Observer
Have you ever felt stupid for copying text blocks and inserting them into a specific file again and again. I have joined yet another project using AWS and in doing so I have gotten myself yet another project where I have to regularly update my ~/.aws/credentials file everytime I switch the AWS account or when the token lifetime has expired.

Basically I fully agree, from a security point of view it does make sense. But in the end I really think having to copy the keys every ~1-2 hours is simply annoying.

## So what was I to do about this?
So I came up with this little tool, which will observe Windows' clipboard buffer and whenever new AWS Credentials have been copied, will recognize them by matching a regex pattern and then immediately write them to the file %USERPROFILE%/.aws/credentials.

### Development Steps

1. Instead of using my own win32 interface, I found [Willy Kimura's](https://github.com/Willy-Kimura/SharpClipboard/commits?author=Willy-Kimura) [SharpClipboard](https://github.com/Willy-Kimura/SharpClipboard), which I was able to easily incorporate into my tool.
2. Modularize the application, allowing the creation of Plugins that can be registered into the tool and be used as subscribers to specific Copy-Content-Types
3. Add Configuration Panels for Plugins

So feel free to use, fork and update the tool with your own ideas and plugins, and create pull-requests, if you have interesting ones ;-)
Thanks in advance!

##### Kind Regards,
###### Ingo


---
###### Special Thanks go to 
- Willy Kimura for providing the [SharpClipboard](https://github.com/Willy-Kimura/SharpClipboard) nuget package and if you like my ClipboardObserver, please also consider to [buy Willy a coffee](https://www.buymeacoffee.com/willykimura). I really like that I can easily use his nuget package without having to wrap my mind about that part of the code.
