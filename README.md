# Microsoft.VisualBasic.Parallel
Parallel library of GCModeller parallel computing


Steps of the distributed computing in this library
1. Create the instance of the TaskInvoke object on your server side program
2. Creates the function to analysis the data

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



Usage of the remote linq script to query remote resource

Imports &lt;namespace>

var source = 
var result = from x as &lt;type> in $source let &ltstatement> where &lt;test_statement> select &lt;ctor.statement>