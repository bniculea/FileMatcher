   M S B A M L   `   `   `  ����F  BFileMatcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:FileMatcherFileMatcher  5   	       FileMatcher.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:FileMatcher     5   	     � Title$  File Matcher��5   	   $	��500��6   $	��525��6+   .��5
      �  � RowDefinitions 5   
   �� 5      $��0.75* �6   �� 5      $��3* �6   �� 5      $��0.35* �6   �� 5      $��0.25* �6   .+�5   
   �� -   5         ��Name$ 
InputPanel��5       �� Orientation$ Vertical=�5      $��0��5      $��5,10,5,5q�5      $��Stretch=�5      .+�5      � $��Center=�6    � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��Auto �6'   �� 5      $��* �6'    5      �� 5      $
��Auto �6$   .+�5       �� $��	Location:��6   $��0��6(   $��0,0,5,0q�68   $
��Left=�5!      �� 5"      -   5         ��Name$ TxtSelectedPath��5"      $	��350��5#      $��1��6#   $��Center=�5$      $
��Left=�5%      �� 5&      -   5        $��BrowseButton��5'      $��Browse��5&      $��2��5(      $��5,0,5,0q�5)      $��50��5*      $
��Left=�5+      5,      5-      ]� 5.      $��0,5,5,5q�6   $��Center=�6)   .+�5/      �� $��Match by extension:��6   $��15,5,5,5q�50      �� 51      -   5        $ TxtExtension��51      $
��.dll��52      $��50��53      $
��left=�54      $��Center=�55      $��0,5,5,5q�56      57      59      � 5:   
   -   5         �Name$ OutputPanel��5:      $��1��6$   .+�5;      &   System.Windows.Controls.DataGrid  -   5          Name$ FileMatchedGridView��5;         CanUserSortColumns . 	   
IsReadOnly	 . ��5<      �� *DataGridCollection+
   RowStyle
 5A      �� 5B       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6    �� Triggers 5C      �� 5D       �� Binding 6+   �� *Equals+ �� Value	 False6F   .a�5E   "   ��  �� Property � �� �� Value
 �  ��6?   5G      5I      5K      5L      �� 5M   
   -   5        $ ControlPanel��5M      $��2��6+   $ 
Horizontal=�5N      $��Center=�5O      .+�5P      �� -   5         ��Name$ ButtonCancel��5P      $��Cancel��5Q      $��Center=�5R      $��Center=�5S      $��50��5T      $��
10, 5,20,5q�5V      �� 5W      -	   5        $ 	ButtonRun��5W      $��50��5X      $	��Run��5Y      $��Center=�5Z      $��	5, 5,20,5q�5\      5]      � 5^   
   -
   5        $ ProgressBarGrid��5^      $��3��5_      $	��5,5q�5`      .+�5a      � -   5         �Name$ ProgressBarStatus��5a       � Minimum$ 0Q�5b       � Maximum$	 100Q�5c      5e      5f      5g      