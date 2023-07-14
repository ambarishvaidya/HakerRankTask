# Hacker News Task

## How to Run

Download the complete solution to your machine and Open it in VS2022.
Update the Nuget packages and build.
Right click on the solution **HakerNewsTask** and select **Properties**.
Configure the items as shown in the figure and click Ok.
![Multiple startup project configuration](/Images/SolutionStartup.png)

Run the solution by running Ctrl + F5 or from the start button ![Run button](\Images\ProjectRun.png).

This will start the Web project at **https://localhost:7268** opening **swagger page in browser**, a **console log of web project** and a **console for Testing**.

Development can be tested by both Swagger and via the Console Utility.

### API

There RESTful methods

1. Get - This returns all the TopSotries Id's _This was not in the document, it is required internally. This can be used to to get an Id for GetStory call._.
1. GetTopStories(int count) - This will return the top **count** items in descending order of their score.
1. GetStory(int id) - Returns the Json formatted string for requested Id.

### Assumptions

- There are few stories having same score. I have taken the first in the list for displaying in top n.
  \_ If there are 3 stories for score 870, then the stories are added to a list, and I am simply taking the first item.
- CommentCount is mapped to Decendants.

### Swagger

![Swagger UI](https://github.com/ambarishvaidya/HakerRankTask/assets/132093368/4b5447f3-14d9-4a7e-8f24-7940c1687e9b)

Using Swagger is simple.

- Click on interested operation
- Click on **Try it out** button.
  - This will change the button to **Cancel**
  - Show an **Execute** button.
  - Enter value if asked.
- Click **Execute**.
- On Success data will be displayed under **Responses**.
- In case of Failure, error will be displayed under **Responses**.

### Console

![Console](https://github.com/ambarishvaidya/HakerRankTask/assets/132093368/657e7f09-dd4a-4710-b007-c43f33578470)

- The console is configured to talk to Web project at **https://localhost:7268**. Any changes to the URL will be required an update and rebuild of client.
- Console will ask to execute
  - **1** for top **X** stories in descending order.
  - **2** for a specific story data
  - **3** to Quit.
- Following images shows sample input and output.

- ![For top 2 stories](https://github.com/ambarishvaidya/HakerRankTask/assets/132093368/6aa69520-ff4f-4ac1-9240-118231124607)

- ![For specific story](https://github.com/ambarishvaidya/HakerRankTask/assets/132093368/dab7e7aa-7650-4ab5-8a1f-0cf7714645bd)

### Logs

- Logging happens in the console that is launched when Solution is run.
- Logging also is done to a file in **C:\temp\HackerNewsApiLogs**

# Enhancements

- From the Practices and patterns, an approach is to reuse HttpClient - https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/ but this is for a later time post rigorous testing.
- Loading the cache when server loads.
  - Currently the cache is loaded on the first request.
  - A timer then keeps the cache udpated **CONTINUOUSLY** post 1 second sleep **_(NOT CONFIGURABLE)_**.
- Memory Profile the solution and optimize it for memory and performance.
- Make changing text configurable.
- More test cases
- Add Mocking test cases
- Benchmarking methods.
- Better Naming conventions
