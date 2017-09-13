Imports System.Collections.Generic
Imports System

Namespace MyIdealProp.Workflow.Core.Model

    Public NotInheritable Class DefaultDefinitions
        Private Sub New()
        End Sub
        Public Shared ReadOnly ParameterProcessId As New DataFieldDefinition() With { _
            .Name = "ProcessId", _
            .Type = GetType(Guid), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterPreviousState As New DataFieldDefinition() With { _
            .Name = "PreviousState", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterCurrentState As New DataFieldDefinition() With { _
            .Name = "CurrentState", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterPreviousStateForDirect As New DataFieldDefinition() With { _
            .Name = "PreviousStateForDirect", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterPreviousStateForReverse As New DataFieldDefinition() With { _
            .Name = "PreviousStateForReverse", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterPreviousActivity As New DataFieldDefinition() With { _
            .Name = "PreviousActivity", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterCurrentActivity As New DataFieldDefinition() With { _
            .Name = "CurrentActivity", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterPreviousActivityForDirect As New DataFieldDefinition() With { _
            .Name = "PreviousActivityForDirect", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterPreviousActivityForReverse As New DataFieldDefinition() With { _
            .Name = "PreviousActivityForReverse", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterCurrentCommand As New DataFieldDefinition() With { _
            .Name = "CurrentCommand", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterExecutedActivityState As New DataFieldDefinition() With { _
            .Name = "ExecutedActivityState", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterConditionResult As New DataFieldDefinition() With { _
            .Name = "ConditionResult", _
            .Type = GetType(Boolean), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterIdentityId As New DataFieldDefinition() With { _
            .Name = "IdentityId", _
            .Type = GetType(Guid), _
            .Scope = DataFieldScope.Persistence _
        }
        Public Shared ReadOnly ParameterImpersonatedIdentityId As New DataFieldDefinition() With { _
            .Name = "ImpersonatedIdentityId", _
            .Type = GetType(Guid), _
            .Scope = DataFieldScope.Persistence _
        }
        Public Shared ReadOnly ParameterSchemeId As New DataFieldDefinition() With { _
            .Name = "SchemeId", _
            .Type = GetType(Guid), _
            .Scope = DataFieldScope.System _
        }
        Public Shared ReadOnly ParameterProcessName As New DataFieldDefinition() With { _
            .Name = "ProcessName", _
            .Type = GetType(String), _
            .Scope = DataFieldScope.System _
        }

        Public Shared ReadOnly ParameterIdentityIds As New DataFieldDefinition() With { _
            .Name = "IdentityIds", _
            .Type = GetType(IEnumerable(Of Guid)), _
            .Scope = DataFieldScope.System _
        }
        Public Shared CommandAuto As New EventDefinition() With { _
            .Name = "Auto" _
        }
        Public Shared CommandSetState As New EventDefinition() With { _
            .Name = "SetState" _
        }


        Public Shared ReadOnly DefaultParameters As IEnumerable(Of DataFieldDefinition) = New List(Of DataFieldDefinition)() From { _
            ParameterProcessId, _
            ParameterPreviousState, _
            ParameterCurrentState, _
            ParameterPreviousStateForDirect, _
            ParameterPreviousStateForReverse, _
            ParameterPreviousActivity, _
            ParameterPreviousActivityForDirect, _
            ParameterPreviousActivityForReverse, _
            ParameterCurrentCommand, _
            ParameterConditionResult, _
            ParameterIdentityId, _
            ParameterImpersonatedIdentityId, _
            ParameterExecutedActivityState, _
            ParameterCurrentActivity, _
            ParameterSchemeId, _
            ParameterIdentityIds, _
            ParameterProcessName _
        }
    End Class
End Namespace
