using System;
using System.Runtime.InteropServices;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Jiggle
{
  [StructLayout(LayoutKind.Sequential)]
  public struct INPUT
  {
    internal uint type;
    internal InputUnion U;
    internal static int Size => Marshal.SizeOf(typeof(INPUT));
  }

  [StructLayout(LayoutKind.Explicit)]
  internal struct InputUnion
  {
    [FieldOffset(0)]
    internal MOUSEINPUT mi;
    [FieldOffset(0)]
    internal KEYBDINPUT ki;
    [FieldOffset(0)]
    internal HARDWAREINPUT hi;
  }
  
  [StructLayout(LayoutKind.Sequential)]
  // ReSharper disable once IdentifierTypo
  internal struct MOUSEINPUT
  {
    internal int dx;
    internal int dy;
    internal int mouseData;
    internal int dwFlags;
    internal uint time;
    internal UIntPtr dwExtraInfo;
  }
  
  [StructLayout(LayoutKind.Sequential)]
  // ReSharper disable once IdentifierTypo
  internal struct KEYBDINPUT
  {
    internal short wVk;
    internal short wScan;
    internal int dwFlags;
    internal int time;
    internal UIntPtr dwExtraInfo;
  }
  
  [StructLayout(LayoutKind.Sequential)]
  // ReSharper disable once IdentifierTypo
  internal struct HARDWAREINPUT
  {
    internal int uMsg;
    internal short wParamL;
    internal short wParamH;
  }
}
