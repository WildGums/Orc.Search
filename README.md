# Orc.Search

Easily add searching to any application.

Uses [Lucene](http://lucenenet.apache.org/) in the background.

Search Syntax: [http://www.lucenetutorial.com/lucene-query-syntax.html](http://www.lucenetutorial.com/lucene-query-syntax.html)


## Downloads

* **[Orc.Search](https://www.nuget.org/packages/Orc.Search/)** => The core of **Orc.Search**.  Containt main services, base classes and attributes.
* **[Orc.Search.Xaml](https://www.nuget.org/packages/Orc.Search.Xaml/)** => Containt basic Ui elements, which could be used for provide serch into your applicetion.

## Features

* used [Lucene](http://lucenenet.apache.org/) for the indexing and searching
* properties, which need to be indexed should be decorated with the **SearchablePropertyAttribute** 
* for support highlighting you sould create your own search highlight providers, by inheriting from **SearchHighlightProviderBase** 
* you can redefine implementation of **ISearchService**, for doing that create class, inherited from **SearchServiceBase** and register it usring **[Catel.IoC.IServiceLocator](http://www.nudoq.org/#!/Packages/Catel.Core/Catel.Core/IServiceLocator)**
* support of asynchronos searching (use **ISearchServiceExtensions**)

## Quick start

* create POCO class for using in search

and decorate properties with **SearchablePropertyAttribute**

	public class Person
    {
    	[SearchableProperty(SearchName = "firstname")]
    	public string FirstName { get; set; }
    
    	[SearchableProperty(SearchName = "lastname")]
    	public string LastName { get; set; }
    
    	public int Age { get; set; }
	}

* Fill **ISearchService** with the data by using **AddObjects()** method. As an argument use collection of *Person*.
* Use **Search()** method for getting search results. As an argument use *string filter*

For using asynchronous version of search. Just use Async suffix at the name of methods (SearchAsync(), AddObjectsAsync(), RemoveObjectsAsync())

