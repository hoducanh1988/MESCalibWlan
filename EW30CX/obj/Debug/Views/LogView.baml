   M S B A M L   `   `   `  ����A  =EW30CX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:EW30CX.ViewsEW30CX.Views  5          EW30CX.Views.LogView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:EW30CX.Views     5      
�������  � 	Resources  5
       �� 
TargetType �� Property   fontSizeNormal �� Value �� Triggers P� Property P� Value  fontSizeButton��%�  '��       7   '	� D     7   '�� �    7  �� 5   
   " ��6   .�5      ��  � ��8   6)   5      �� 5   
   " 	�6   .�5      ��  � ��8   6)   �� 5       � ��$ 25,5,0,0q�6'   �� 5       � ��$ Normal"�6+   �� 5       � ��
 �   �6+    5      P� 5       �@d�	IsChecked$
 TrueY�60   . �5      ��  � ��
 �  ��63   �� 5       � ��$ SemiBold"�63   5      5      �� 5   
   " ��6   .�5      ��  � ��$	 100��6&   �� 5       � ��$ 35��6'   �� 5       � ��$ 0,15,0,0q�6'   �� 5       � ��8   6)   �� 5       � ��$ SemiBold"�6+   �� 5        � ��$
 Left=�64   5!      5"      .��5$      �� $��5,0,5,0q�6   .+�5%   
   �� $;��5Vui lòng chọn loại dữ liệu cần truy vấn:��6   	� 5&   
   $��Setting file��6c    d� 	IsChecked 6   ��  �� Path$ LM.isSetting�	 �� UpdateSourceTrigger$	 PropertyChanged=�6c   	� 5'   
   $��Software config file��6d    6   �� $ LM.isSoftware�$	 PropertyChanged=�6d   	� 5(   
   $��Golden file��6b    6   �� $ LM.isGolden�$	 PropertyChanged=�6b   	� 5)   
   $��
Log folder��6_    6   �� $ LM.isLog�$	 PropertyChanged=�6_   	� 5*   
   $��QPST Server��6`    6   �� $ 	LM.isQPST�$	 PropertyChanged=�6`   �� 5+   
   $��17(�6   $��Go��6=   ��6   �� *	GoCommand+6=   5-      5/      