# Win Systems DEV Challenge 2023

This is my own version of the Win Systems Dev Challenge 2023.

I received the challenge on  October 27, 2023.

## Table of contents
* [Authors](#authors)
* [General Info](#general-info)
* [Quick Start](#quick-start)
* [Technologies](#technologies)
* [Usage](#usage)
* [License info](#licence-info)

## Authors

- [Xavier Sánchez Atienza](https://www.linkedin.com/in/xatienza)

## General info

I've created two projects with .NET 6 to develop the Win Systems challenge:

* The first is a library project in which I have developed all the application logic.
* The second project is a test unit project with all the tests to check the library project.

## Quick Start

To clone and run my challenge, you'll need [Git](https://git-scm.com/) and [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed on your computer. From your linux, macOS or Windows command line:

```(bash)

    # Clone this repository
    
    $ git clone git@github.com:xatienza/winsystems-challenge.git
    
    # Restore libraries
    
    dotnet restore
    
    # Build the solution
    
    dotnet build
    
    # Test my challenge
    
    dotnet test

```

## Technologies

Project is created with:

* .NET 6. .NET 6 SDK 
* [RestEase Library](https://github.com/canton7/RestEase). This library helps me to manage the HttpClient.
* [xUnit](https://xunit.net/). xUnit.net is a free, open source, community-focused unit testing tool for the .NET Framework.

## Usage

There two options to try my challenge:

* **Dotnet**. The main option to test my challenge is with a set of unit tests.

```(C#)

    # Restore libraries

    dotnet restore

    # Build the solution

    dotnet build

    # Run all the unit test linked to the solution:

    dotnet test

```

* **Docker**. You can use docker to test my challenge. First, you have to build the image and then you have to run it to see the result.


```(docker)
    # Build docker image:

    docker build -t winsystems-challenge:latest .

    # Run all the test inside the docker image:

    docker run winsystems-challenge:latest

```

## License info
MIT License

Copyright (c) 2018 Moringa School

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
