# ltse_exercise

Build this project in Visual Studio or with command-line C# compiler.

The program expects the path of the file containing the orders (e.g. trades.csv) as a command-line argument. If you are using Visual Studio, the easiest way to do this is to dump the file under `ltseExercise\ltseExercise\bin\Debug` and then under Project -> Properties -> Debug, set the command line argument to be the name of the file. 

After the program executes you will find 4 files in `ltseExercise\ltseExercise\bin\Debug`:

* __accepted.txt__ - contains broker,id pairs for all accepted orders
* __rejected.txt__ - contains broker,id pairs for all rejected orders
* __accepted.json__ - contains all accepted order details in JSON format
* __rejected.json__ - contains all rejected order details in JSON format
