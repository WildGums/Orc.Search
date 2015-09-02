Orc.Search
==============

[![Join the chat at https://gitter.im/WildGums/Orc.Search](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/WildGums/Orc.Search?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![License](https://img.shields.io/github/license/wildgums/orc.search.svg)
![NuGet downloads](https://img.shields.io/nuget/dt/orc.search.svg)
![Version](https://img.shields.io/nuget/v/orc.search.svg)
![Pre-release version](https://img.shields.io/nuget/vpre/orc.search.svg)

Easily add searching to any application.

Uses [Lucene](http://lucenenet.apache.org/) in the background.

Search Syntax: [http://www.lucenetutorial.com/lucene-query-syntax.html](http://www.lucenetutorial.com/lucene-query-syntax.html)


Nuget Packages
-----------------

- **[Orc.Search](https://www.nuget.org/packages/Orc.Search/)** => The core of **Orc.Search**.  Contains the main services, base classes and attributes.
- **[Orc.Search.Xaml](https://www.nuget.org/packages/Orc.Search.Xaml/)** => Containt basic Ui elements, which can be used to add seach functionality to your application.

Features
--------

- Uses [Lucene](http://lucenenet.apache.org/) for the indexing and searching
- Properties, which need to be indexed should be decorated with the **SearchablePropertyAttribute** 
- To support highlighting you should create your own search highlight provider, by inheriting from **SearchHighlightProviderBase** 
- You can redefine the implementation of **ISearchService**, by creating your own class which will inherit **SearchServiceBase** and register it using **[Catel.IoC.IServiceLocator](http://www.nudoq.org/#!/Packages/Catel.Core/Catel.Core/IServiceLocator)**
- Supports asynchronos searching (uses **ISearchServiceExtensions**)

Quick start
---------------

1. Create a POCO class to use in your search and decorate the properties with *SearchablePropertyAttribute*
 
		public class Person
		{
			[SearchableProperty(SearchName = "firstname")]
			public string FirstName { get; set; }
			
			[SearchableProperty(SearchName = "lastname")]
			public string LastName { get; set; }
			
			public int Age { get; set; }
		}


2. Fill the *ISearchService* with the appropriate "Person" data using the *AddObjects()* method. The objects must be wrapped inside an *ISearchable* implementation, for example the *ReflectionSearchable*:

		searchService.AddObjects(persons.Select(x => new ReflectionSearchable(x));

- Use the *Search()* method for getting search results. Use the *string filter* as an argument. Note that you will receive the *ISearchable* instances that have a hit. This means you can still retrieve the metadata after a search has been completed.

In order to use the asynchronous version of search. Just use the Async suffix method names (i.e. SearchAsync(), AddObjectsAsync(), RemoveObjectsAsync())
