< Project Sdk = "Microsoft.NET.Sdk.Web" >
  < PropertyGroup >
    < TargetFramework > net8.0 </ TargetFramework >
    < TypeScriptCompileBlocked > true </ TypeScriptCompileBlocked >
    < IsPackable > false </ IsPackable >
  </ PropertyGroup >

  < ItemGroup >
    < None Remove = "**\*" />
    < Content Include = "**\*" Exclude = "node_modules\**\*;build\**\*" />
  </ ItemGroup >
</ Project >
