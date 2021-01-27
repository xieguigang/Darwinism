#Region "Microsoft.VisualBasic::97228bd4c54ddc43bb83da5180100d9b, Google.Protobuf\WellKnownTypes\Api\Mixin.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Class Mixin
    ' 
    '         Properties: Descriptor, DescriptorProp, Name, Parser, Root
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>
    '''  Declares an API to be included in this API. The including API must
    '''  redeclare all the methods from the included API, but documentation
    '''  and options are inherited as follows:
    '''
    '''  - If after comment and whitespace stripping, the documentation
    '''    string of the redeclared method is empty, it will be inherited
    '''    from the original method.
    '''
    '''  - Each annotation belonging to the service config (http,
    '''    visibility) which is not set in the redeclared method will be
    '''    inherited.
    '''
    '''  - If an http annotation is inherited, the path pattern will be
    '''    modified as follows. Any version prefix will be replaced by the
    '''    version of the including API plus the [root][] path if specified.
    '''
    '''  Example of a simple mixin:
    '''
    '''      package google.acl.v1;
    '''      service AccessControl {
    '''        // Get the underlying ACL object.
    '''        rpc GetAcl(GetAclRequest) returns (Acl) {
    '''          option (google.api.http).get = "/v1/{resource=**}:getAcl";
    '''        }
    '''      }
    '''
    '''      package google.storage.v2;
    '''      service Storage {
    '''        rpc GetAcl(GetAclRequest) returns (Acl);
    '''
    '''        // Get a data record.
    '''        rpc GetData(GetDataRequest) returns (Data) {
    '''          option (google.api.http).get = "/v2/{resource=**}";
    '''        }
    '''      }
    '''
    '''  Example of a mixin configuration:
    '''
    '''      apis:
    '''      - name: google.storage.v2.Storage
    '''        mixins:
    '''        - name: google.acl.v1.AccessControl
    '''
    '''  The mixin construct implies that all methods in `AccessControl` are
    '''  also declared with same name and request/response types in
    '''  `Storage`. A documentation generator or annotation processor will
    '''  see the effective `Storage.GetAcl` method after inherting
    '''  documentation and annotations as follows:
    '''
    '''      service Storage {
    '''        // Get the underlying ACL object.
    '''        rpc GetAcl(GetAclRequest) returns (Acl) {
    '''          option (google.api.http).get = "/v2/{resource=**}:getAcl";
    '''        }
    '''        ...
    '''      }
    '''
    '''  Note how the version in the path pattern changed from `v1` to `v2`.
    '''
    '''  If the `root` field in the mixin is specified, it should be a
    '''  relative path under which inherited HTTP paths are placed. Example:
    '''
    '''      apis:
    '''      - name: google.storage.v2.Storage
    '''        mixins:
    '''        - name: google.acl.v1.AccessControl
    '''          root: acls
    '''
    '''  This implies the following inherited HTTP annotation:
    '''
    '''      service Storage {
    '''        // Get the underlying ACL object.
    '''        rpc GetAcl(GetAclRequest) returns (Acl) {
    '''          option (google.api.http).get = "/v2/acls/{resource=**}:getAcl";
    '''        }
    '''        ...
    '''      }
    ''' </summary>
    Partial Public NotInheritable Class Mixin
        Implements IMessageType(Of Mixin)

        Private Shared ReadOnly _parser As MessageParserType(Of Mixin) = New MessageParserType(Of Mixin)(Function() New Mixin())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of Mixin)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.ApiReflection.Descriptor.MessageTypes(2)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As pbr.MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As Mixin)
            Me.New()
            name_ = other.name_
            root_ = other.root_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As Mixin Implements IDeepCloneable(Of Mixin).Clone
            Return New Mixin(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""
        ''' <summary>
        '''  The fully qualified name of the API which is included.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Name As String
            Get
                Return name_
            End Get
            Set(value As String)
                name_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "root" field.</summary>
        Public Const RootFieldNumber As Integer = 2
        Private root_ As String = ""
        ''' <summary>
        '''  If non-empty specifies a path under which inherited HTTP paths
        '''  are rooted.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Root As String
            Get
                Return root_
            End Get
            Set(value As String)
                root_ = CheckNotNull(value, "value")
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, Mixin))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As Mixin) As Boolean Implements IEquatable(Of Mixin).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Not Equals(Root, other.Root) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            If Root.Length <> 0 Then hash = hash Xor Root.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Name.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(Name)
            End If

            If Root.Length <> 0 Then
                output.WriteRawTag(18)
                output.WriteString(Root)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            If Root.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Root)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As Mixin) Implements IMessageType(Of Mixin).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            If other.Root.Length <> 0 Then
                Root = other.Root
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Name = input.ReadString()

                    Case 18
                        Root = input.ReadString()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace
