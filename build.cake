var projectName = "Orc.Search";
var projectsToPackage = new [] { "Orc.Search", "Orc.Search.Xaml" };
var company = "WildGums";
var startYear = 2010;
var defaultRepositoryUrl = string.Format("https://github.com/{0}/{1}", company, projectName);

#l "./deployment/cake/variables.cake"
#l "./deployment/cake/tasks.cake"
