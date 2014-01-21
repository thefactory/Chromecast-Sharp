using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libGCKFramework.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, ForceLoad = true)]
