# Clipboard Observer
Have you ever felt stupid for copying text blocks and inserting them into a specific file again and again. I have joined yet another project using AWS and in doing so I have gotten myself yet another project where I have to regularly update my ~/.aws/credentials file everytime the token lifetime has expired.

Basically that is not a bad thing and I think it is a good addition to security. But I really think copying the keys every ~2 hours is stupid.
## So what was I to do about this?
Based on that I thought about creating a small tool, that allows me to recognize when those credentials have been added to Windows' clipboard buffer and then immediately have the tool overwrite the aws credentials file with the new keys.
### Development Steps

1. Instead of using my own win32 interface, I found [Willy Kimura's](https://github.com/Willy-Kimura/SharpClipboard/commits?author=Willy-Kimura) [SharpClipboard](https://github.com/Willy-Kimura/SharpClipboard), which I was able to easily incorporate into my tool.
2. Modularize the application, allowing the creation of Plugins that can be registered into the tool and be used as subscribers to specific Copy-Content-Types
3. Add Configuration Panels for Plugins

### Next Steps
Right now I am thinking about another text-content plugin, which will launch user defined regex-checks on newly added text items in the copy buffer and then allow different reactions like e. g.
- replace-file-with-content
- append-content-to-file
- open-content-in-app-as-new-doument
I am sure, I will have more ideas, but I am not yet sure which idea would be how feasible as of yet. Let's just wait a bit and see, what I can come up with.

So feel free to use, fork and update the tool with your own ideas and plugins, and create pull-requests, if you have interesting ones ;-)
Thanks in advance!

##### Kind Regards,
###### Ingo


---
###### Special Thanks go to 
- Willy Kimura for providing the [SharpClipboard](https://github.com/Willy-Kimura/SharpClipboard) nuget package and if you like my ClipboardObserver, please consider to [buy Willi a coffee](https://www.buymeacoffee.com/willykimura). I really like that I can easily use his nuget package without having to wrap my mind about that part of the code.
