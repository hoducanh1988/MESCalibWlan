   M S B A M L   `   `   `  ����A  =EW30SX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:EW30SX.ViewsEW30SX.Views  5          EW30SX.Views.LoginView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:EW30SX.Views     5      .��5
      ��   �� CornerRadius$  5��6   ���#FFFFFF$��0q�63   ._�5   
   � .+�5      �� $��	0,-30,0,0q�6   $  5��6(   ���	#FF039BE5$��1q�6Q   $��Center=�6e   $��Center=�6�   $	��150��6�   $	��250��6�   ._�5      �  � ColumnDefinitions 5      �� 5      $	��180 �6+   �� 5      $��* �6+   .+�5      �� ���#343132$  5,0,0,5��63   $��0��6J   ._�5      �� .+�5      �� 
�������$��17(�67   $��SemiBold"�6E   $��Authorization��6[   �� 5      
�������$��12(�67   $
��User��6E   �� 5       �� Orientation$ 
Horizontal=�6)   .+�5   "   �� $��0��6*   $��SemiBold"�67   
����  �$��Center=�6_   $��5,0,0,0q�6�   $��5,0,0,0q�6�   ���	#FFEEEEEE$	��160��6�   ��5   *   ��  �� Path$ LM.User� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�.��6z    J� InputBindings 5   &   �� 5   *   
 �� Key$ Enter��6S    �� Command 65   �� *	GoCommand+6S   .��5   =   5   #   �� 5   "   $��	-20,0,0,0q�6(   $��20��6;   $��/Asset/Image/user.png��6F   5      �� 5      
�������$��12(�67   $��Password��6E   �� 5       $ 
Horizontal=�6)   .+�5!   "   G� $��1��6.   $��SemiBold"�6;   
����  �$��Center=�6c   $��5,0,0,0q�6�   $��5,0,0,0q�6�   ���	#FFEEEEEE$	��160��6�   '   !EW30SX.Views.PasswordBoxAssistant	   BindPassword	 . 
   BoundPassword
 5#   .   �� $ LM.Password�$ PropertyChanged=�$ TwoWay=� 5%   &   �� 5&   *   $ Enter��6S    65   �� *	GoCommand+6S   5(   #   �� 5)   "   $��	-20,0,0,0q�6(   $��20��6;   $��/Asset/Image/password.png��6F   6k   5*      5+      5,      �� 5-      
������ $  0,5,5,0��66   $��1��6M   ._�5.      �� $	��150��6?   ���#35BBD9��6!   �� *	GoCommand+��5/      � 50   "   .+�51   &   �� $��-10,-30,0,0q�6-   $��40��6B   $��/Asset/Image/next.png��6M   �� 52   &   $��20(�6,   ���	#FFF5F5F5$
��Bold"�6Q   $��
-10,80,0,0q�6c   $��GO��6w   53   #   55      56      57      58      �� 5:      
���  ��$��Center=�6%   $��Center=�6B   $��SemiBold"�6]   $��17(�6s   $��	0,170,0,0q�6�   ��6�   �� $ LM.errorMessage�$ PropertyChanged=�5;      5<      5>      