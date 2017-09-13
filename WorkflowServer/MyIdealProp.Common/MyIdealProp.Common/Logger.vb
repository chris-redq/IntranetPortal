Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports log4net
Imports log4net.Config


Namespace MyIdealProp.Common
	Public NotInheritable Class Logger
		Private Sub New()
		End Sub
		Private Shared ReadOnly m_log As ILog = LogManager.GetLogger(GetType(Logger))
		Private Shared _configured As Boolean
		Private Shared _lock As New Object()
		Public Shared ReadOnly Property Log() As ILog
			Get
				If Not _configured Then
					SyncLock _lock
						If Not _configured Then
							InitLogger()
							_configured = True
						End If
					End SyncLock
				End If
				Return m_log
			End Get
		End Property

		Public Shared Sub InitLogger()
			XmlConfigurator.Configure()
		End Sub
	End Class
End Namespace
