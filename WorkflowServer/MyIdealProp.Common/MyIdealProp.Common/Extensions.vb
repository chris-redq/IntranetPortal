Imports System.Collections
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Web
Imports System.Xml.Linq
Imports System
Namespace MyIdealProp.Common

    Public NotInheritable Class Extensions

        Private Sub New()
        End Sub

        Public Shared Function SingleOrDefault(element As XElement, name As String) As XElement
            Return If(element.Name = name, element, element.Elements(name).SingleOrDefault())
        End Function

        Public Shared Function Body(request As HttpRequestBase) As String
            Dim body__1 As String

            Using reader = New StreamReader(request.InputStream)
                request.InputStream.Position = 0
                body__1 = reader.ReadToEnd()
            End Using

            Return body__1
        End Function

        Public Shared Function ToNullableType(type As Type) As Type
            Dim newType = If(Nullable.GetUnderlyingType(type), type)
            Return If(newType.IsValueType, GetType(Nullable(Of )).MakeGenericType(newType), newType)
        End Function


        Public Shared Function IsNullable(type As Type) As Boolean
            Return Nullable.GetUnderlyingType(type) IsNot Nothing
        End Function


        Public Shared Function GetUnderlyingType(type As Type) As Type
            Return If(Nullable.GetUnderlyingType(type), type)
        End Function


        Public Shared Function GetDefaultValue(type As Type) As Object
            If type.IsValueType Then
                Return Activator.CreateInstance(type)
            End If
            Return Nothing
        End Function


        Public Shared Function ToLowerCaseString(value As Boolean) As String
            Return value.ToString(CultureInfo.InvariantCulture).ToLower()
        End Function


        Public Shared Function ExtendedEquals(value As Object, valueToCompare As Object) As Boolean
            If valueToCompare Is Nothing Then
                Return value Is Nothing
            End If

            If value.[GetType]() Is valueToCompare.[GetType]() AndAlso TypeOf value Is IEnumerable Then
                Dim valueArray = TryCast(value, IEnumerable).Cast(Of Object)().ToArray()
                Dim valueToCompareArray = TryCast(valueToCompare, IEnumerable).Cast(Of Object)().ToArray()

                If valueArray.Length <> valueToCompareArray.Length Then
                    Return False
                End If

                For i As Integer = 0 To valueArray.Length - 1
                    If valueArray(i) Is Nothing AndAlso valueToCompareArray(i) IsNot Nothing Then
                        Return False
                    End If
                    If valueArray(i) IsNot Nothing AndAlso valueToCompareArray(i) Is Nothing Then
                        Return False
                    End If
                    If valueArray(i) IsNot Nothing AndAlso valueToCompareArray(i) IsNot Nothing Then
                        If Not valueArray(i).Equals(valueToCompareArray(i)) Then
                            Return False
                        End If
                    End If
                Next

                Return True
            End If

            Return value.Equals(valueToCompare)
        End Function
    End Class
End Namespace
