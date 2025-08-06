using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class MessageCard : UserControl
    {
        Member member;
        public MessageCard(OverdueMessage o)
        {
            InitializeComponent();
            Header.Text = "Overdue Item";
            Details.Text = o.details;
            Expand_Btn.IsEnabled = false;
        }
        public MessageCard(DueDateMessage d)
        {
            InitializeComponent();
            Header.Text = "Due Date Upcoming";
            Details.Text = d.details;
        }

        public MessageCard(ReturnedMessage r)
        {
            InitializeComponent();
            Header.Text = "Returned Item";
            Details.Text = r.details;
        }
        public MessageCard(Member _member) //Message cards are also used to display the list of all member accounts registered (admin function).
        {
            InitializeComponent();
            member = _member;
            Header.Text = $"Name: {member.name} (id:{member.id})";
            Details.Text = $"DOB:   ({member.DOB})\nUsername:   {member.username}\nPassword:   {member.password}";
        }

        void Edit_Click(Object sender, RoutedEventArgs e)
        {
            PageLoader.UpdateMemberDetails(member);
        }


    }
}
