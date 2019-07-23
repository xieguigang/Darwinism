Namespace DistributeServices

    Public Enum RegistryProtocols
        ''' <summary>
        ''' Send a ping to ask if it is a grid node?
        ''' </summary>
        IsANode
        ''' <summary>
        ''' Send a ping to ask if it is a registry of the <see cref="CenterController"/>
        ''' </summary>
        IsAPool
        ''' <summary>
        ''' A grid node sign up and registry in this computing grid
        ''' </summary>
        SignUp
    End Enum
End Namespace