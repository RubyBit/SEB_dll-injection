# DLL Injection in a .NET Framework process

Showcase video: https://www.youtube.com/watch?v=Yd-9hMPuc78

We showcase a method to inject a .NET framework dll into a .NET framework process using a bootstrap native dll and 
the ovewrite of functions in the process a runtime. 

This is showcased using SEB (the safe exam browser) which allows instituions to hold virtual exams without the students cheating. 
Using this solution though, we are able to bypass a number of key checks and launch arbitrary processes in the desktop which SEB is running.

The version of SEB which the method is tested on is 3.5.0 (the currently latest one) but don't expect the method to work in future versions of the application.


A reminder that this repository is only for educational purposes.


## Projects

- Tester (a simple .NET framework app to test the essence of the method)
- Bootstrapper (This app (a native dll) is responsible for the initial injection and setup of the environment such that a .NET framework dll can be injected and launched)
- OldMmanSEB (A .NET framework dll) is responsible for the actual patching of the functions of the target .NET framework process (using HarmonyLib but can be done natively through reflection).

