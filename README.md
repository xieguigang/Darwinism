# Microsoft.VisualBasic.Parallel
Parallel library of GCModeller parallel computing


Steps of the distributed computing in this library
	1. Create the instance of the TaskInvoke object on your server side program
	2. Creates the function to analysis the data
	3. Create a TaskHost object on your local client, and connect to the remote through IPEndPoint
	4. Using AddressOf function to gets the function Delegate pointer of your target function which is want to running on the remote.
	5. Calling this function Delegate pointer on the remote machine through TaskHost.Invoke and gets the returns result.

Through these steps, your function on the local client should be running on the cloud and you are able to integrated your cloud server calculation resource onto your local client app to provides the powerfull cloud computing feature for your application.

Important NOTE:
1. Please notices that the function which is running on the remote machine should be a statics method and donot reference to the module variable as the module variable is probably not initialized on the remote machine, just using the variable limits in your function inner local variable.
Example:

    Module Test1
	    Dim m_var As Integer
    
        ' As the m_var is a module variable, and this variable is probley is not initialized on the remote machine
        ' So that this function is always returns ZERO
        ' So that reference to the module variable is not recommended
        Public Function MayFailure As Integer
            Return Test1.m_var
        End Function
    
        ' As all of the variable can be initialized in the function body, 
        ' so that this function is running in the correctly state
        Public Function IsSuccess As Integr
        	Dim var As Integer
            Return var
        End Function
    End Module
    
2. The object of the remote function pointer its parameter should be a simple class, which is the Class object instance can be serialization and deserialization by the json through the property, if the class is initialize by a method, and then this class is not a "simple" class, Json transfer of this class and create instance at the remote machine could be failure or running in a unexpected result. 
3. Currently this library just support the statics method, but the instance method will be supported in the feature works.










Usage of the remote linq script to query remote resource

Imports &lt;namespace>

var source = 
var result = from x as &lt;type> in $source let &ltstatement> where &lt;test_statement> select &lt;ctor.statement>