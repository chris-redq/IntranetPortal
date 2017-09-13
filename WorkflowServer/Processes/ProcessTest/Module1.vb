Imports PortalProcesses

Module Module1

    Sub Main()
        'Dim t = GetType(PortalProcesses.TaskProcessAction)
        'Console.WriteLine(t.AssemblyQualifiedName)
        GetData()
    End Sub

    Sub GetData()
        Dim result = MyIdealProp.Workflow.DBPersistence.Worklist.GetUserWorklistReport("Chris Yan")
    End Sub

End Module
