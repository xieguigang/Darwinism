﻿#Region "Microsoft.VisualBasic::2e2c6dd8c6153509b6d949ad6c036af4, XDRStream\Writing\WriteBuilder.EmitDynWriter.vb"

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

'     Class WriteBuilder
' 
'         Function: EmitDynWriter
' 
'         Sub: EmitOverride_WriteTMany, EmitOverride_WriteTOne
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Emit
Imports System.Reflection
Imports System.Reflection.Emit
Imports Microsoft.VisualBasic.Data.IO

Namespace Writing

    Partial Public NotInheritable Class WriteBuilder
        Private Function EmitDynWriter() As Type
            Dim typeBuilder = _modBuilder.DefineType("DynWriter", TypeAttributes.NotPublic Or TypeAttributes.Class Or TypeAttributes.Sealed, GetType(Writer))
            Dim fb_mapperInstance = typeBuilder.DefineField("Mapper", GetType(WriteMapper), FieldAttributes.Public Or FieldAttributes.Static)
            Dim ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, New Type() {GetType(IByteWriter)})
            Dim ilCtor As ILGenerator = ctor.GetILGenerator()
            ilCtor.Emit(OpCodes.Ldarg_0)
            ilCtor.Emit(OpCodes.Ldarg_1) ' reader
            ilCtor.Emit(OpCodes.Call, GetType(Writer).GetConstructor(BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Type() {GetType(IByteWriter)}, Nothing))
            ilCtor.Emit(OpCodes.Ret)
            EmitOverride_WriteTOne(typeBuilder, fb_mapperInstance)
            EmitOverride_WriteTMany(typeBuilder, "CacheWriteFix", _fixCacheDescription, fb_mapperInstance)
            EmitOverride_WriteTMany(typeBuilder, "CacheWriteVar", _varCacheDescription, fb_mapperInstance)
            Return typeBuilder.CreateType()
        End Function

        Private Sub EmitOverride_WriteTOne(typeBuilder As TypeBuilder, mapperInstance As FieldInfo)
            Dim miDeclaration = GetType(Writer).GetMethod("CacheWrite", BindingFlags.NonPublic Or BindingFlags.Instance)
            Dim mb = typeBuilder.DefineMethod("CacheWrite", MethodAttributes.Family Or MethodAttributes.Virtual)
            Dim genTypeParam = mb.DefineGenericParameters("T")(0)
            mb.SetReturnType(Nothing)
            mb.SetParameters(genTypeParam)
            typeBuilder.DefineMethodOverride(mb, miDeclaration)
            Dim fi = TypeBuilder.GetField(_oneCacheDescription.Result.MakeGenericType(genTypeParam), _oneCacheDescription.Result.GetField("Instance"))
            Dim il As ILGenerator = mb.GetILGenerator()
            Dim noBuild As Label = il.DefineLabel()
            il.Emit(OpCodes.Ldsfld, fi)
            il.Emit(OpCodes.Brtrue, noBuild)
            il.Emit(OpCodes.Ldsfld, mapperInstance)
            il.Emit(OpCodes.Call, GetType(WriteMapper).GetMethod("BuildCaches", BindingFlags.Public Or BindingFlags.Instance))
            il.MarkLabel(noBuild)
            il.Emit(OpCodes.Ldsfld, fi)
            il.Emit(OpCodes.Ldarg_0) ' this writer
            il.Emit(OpCodes.Ldarg_1) ' item
            Dim miInvoke = TypeBuilder.GetMethod(GetType(WriteOneDelegate(Of)).MakeGenericType(genTypeParam), GetType(WriteOneDelegate(Of)).GetMethod("Invoke"))
            il.Emit(OpCodes.Callvirt, miInvoke)
            il.Emit(OpCodes.Ret)
        End Sub

        Private Shared Sub EmitOverride_WriteTMany(tb As TypeBuilder, name As String, manyCacheDesc As StaticCacheDescription, mapperInstance As FieldInfo)
            Dim miDeclaration = GetType(Writer).GetMethod(name, BindingFlags.NonPublic Or BindingFlags.Instance)
            Dim mb = tb.DefineMethod(name, MethodAttributes.Family Or MethodAttributes.Virtual)
            Dim genTypeParam = mb.DefineGenericParameters("T")(0)
            mb.SetReturnType(Nothing)
            mb.SetParameters(GetType(UInteger), genTypeParam)
            tb.DefineMethodOverride(mb, miDeclaration)
            Dim fi = manyCacheDesc.Instance(genTypeParam)
            Dim il As ILGenerator = mb.GetILGenerator()
            Dim noBuild As Label = il.DefineLabel()
            il.Emit(OpCodes.Ldsfld, fi)
            il.Emit(OpCodes.Brtrue, noBuild)
            il.Emit(OpCodes.Ldsfld, mapperInstance)
            il.Emit(OpCodes.Call, GetType(WriteMapper).GetMethod("BuildCaches", BindingFlags.Public Or BindingFlags.Instance))
            il.MarkLabel(noBuild)
            il.Emit(OpCodes.Ldsfld, fi)
            il.Emit(OpCodes.Ldarg_0)  ' this writer
            il.Emit(OpCodes.Ldarg_1) ' len or max
            il.Emit(OpCodes.Ldarg_2) ' item
            Dim miInvoke = TypeBuilder.GetMethod(GetType(WriteManyDelegate(Of)).MakeGenericType(genTypeParam), GetType(WriteManyDelegate(Of)).GetMethod("Invoke"))
            il.Emit(OpCodes.Callvirt, miInvoke)
            il.Emit(OpCodes.Ret)
        End Sub
    End Class
End Namespace