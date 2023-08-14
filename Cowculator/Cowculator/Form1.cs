using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;//added this for writing to and reading from file

namespace Cowculator
{
    public partial class Form1 : Form
    {
        //Global Vars
        string filenm = "CowData.txt";

        decimal hankHerd = 0.00m; //       --> They enter 33.3333 which i multiply by 0.01 to set hankHerd to
        decimal markHerd = 0.00m;//             This is so it would round up if I round to nearest 2 dec places then the printed
        decimal billHerd = 0.00m;//             value is *100 before printed to display 33.3333 to the user, while I store 0.333333
        int totalHerd = 0;

        decimal hankOwes = 0.00m;
        decimal markOwes = 0.00m;
        decimal billOwes = 0.00m;

        decimal owedToHank = 0.00m;
        decimal owedToMark = 0.00m;
        decimal owedToBill = 0.00m;
        
        decimal hankProfit = 0.00m;
        decimal markProfit = 0.00m;
        decimal billProfit = 0.00m;

        //Percent Arrays
        private decimal[] HanksPercents = new decimal[25];
        private decimal[] MarksPercents = new decimal[25];
        private decimal[] BillsPercents = new decimal[25];

        int CowPurchasesMade = 0;//which index in the percent arrays you're in

        decimal communalFund = 0.00m;

        bool ThemeSelected;//false == light theme  /  true == dark theme

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Text = "How may cows to sell: ";
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            StreamReader reader;
            string cowReadItem;
            try
            {
                reader = new StreamReader(filenm);

                //Read and restore first row of stats table
                cowReadItem = reader.ReadLine();
                hankOwes = decimal.Parse(cowReadItem);
                HankOwes_lbl.Text = cowReadItem;

                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                markOwes = decimal.Parse(cowReadItem);
                MarkOwes_lbl.Text = cowReadItem;

                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                billOwes = decimal.Parse(cowReadItem);
                BillOwes_lbl.Text = cowReadItem;


                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                owedToHank = decimal.Parse(cowReadItem);
                OwedToHank_lbl.Text = cowReadItem;

                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                owedToMark = decimal.Parse(cowReadItem);
                OwedToMark_lbl.Text = cowReadItem;

                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                owedToBill = decimal.Parse(cowReadItem);
                OwedToBill_lbl.Text = cowReadItem;


                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                hankProfit = decimal.Parse(cowReadItem);
                HankProfit_lbl.Text = cowReadItem;

                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                markProfit = decimal.Parse(cowReadItem);
                MarkProfit_lbl.Text = cowReadItem;

                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                billProfit = decimal.Parse(cowReadItem);
                BillProfit_lbl.Text = cowReadItem;


                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                hankHerd_lbl.Text = cowReadItem;

                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                markHerd_lbl.Text = cowReadItem;

                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                billHerd_lbl.Text = cowReadItem;

                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                totalHerdNum_lbl.Text = cowReadItem;
                totalHerd = int.Parse(cowReadItem);


                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                comFund_lbl.Text = cowReadItem;
                communalFund = decimal.Parse(cowReadItem);


                //Read hankHerd
                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                hankHerd = decimal.Parse(cowReadItem);

                //Read markHerd
                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                markHerd = decimal.Parse(cowReadItem);

                //Read billHerd
                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                billHerd = decimal.Parse(cowReadItem);


                cowReadItem = reader.ReadLine();
                cowReadItem = reader.ReadLine();
                CowPurchasesMade = int.Parse(cowReadItem);

                cowReadItem = reader.ReadLine();//Reading Hank's Percents of cattle purchases back into array
                for (int count = 0; count < 25; count++)
                {
                    cowReadItem = reader.ReadLine();
                    HanksPercents[count] = decimal.Parse(cowReadItem);
                }


                cowReadItem = reader.ReadLine();//deal with the blank line between hanks array and marks array
                for (int count = 0; count < 25; count++)
                {
                    cowReadItem = reader.ReadLine();
                    MarksPercents[count] = decimal.Parse(cowReadItem);
                }

                cowReadItem = reader.ReadLine();//deal with the blank line between hanks array and marks array
                for (int count = 0; count < 25; count++)
                {
                    cowReadItem = reader.ReadLine();
                    BillsPercents[count] = decimal.Parse(cowReadItem);
                }


                reader.Close();//Closes reader when done reading from the file.
            }
            catch
            {
                MessageBox.Show("No Previously Saved Data Could Be Accessed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void sale_btn_Click(object sender, EventArgs e)
        {
            int cowsSold = 0;
            decimal salePrice = 0.00m;

            //Error Check Cows To Sell Box
            if ((int.TryParse(CowsToSell_txt.Text, out cowsSold) == false || int.Parse(CowsToSell_txt.Text) <= 0) )//if they enter non numeric or negative data
            {
                //display an error message and reprompt them for new data
                if (CowsToSell_txt.Text == "")
                {
                    MessageBox.Show("You must enter a value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(int.TryParse(CowsToSell_txt.Text, out cowsSold) == false)//non numeric
                {
                    MessageBox.Show("You must enter an integer.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(int.Parse(CowsToSell_txt.Text) <= 0) //if < 0
                {
                    MessageBox.Show("You must enter a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            //Error Check Sale Price Box
            else if ((decimal.TryParse(SalePrice_txt.Text, out salePrice) == false || double.Parse(SalePrice_txt.Text) <= 0.00))//if they enter non numeric or negative data
            {
                try
                {
                    //display an error message and reprompt them for new data
                    if (SalePrice_txt.Text == "")
                    {
                        MessageBox.Show("You must enter a value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (decimal.TryParse(SalePrice_txt.Text, out salePrice) == false)//non numeric
                    {
                        MessageBox.Show("You must enter a numeric value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (int.Parse(SalePrice_txt.Text) <= 0.00) //if < 0
                    {
                        MessageBox.Show("You must enter a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sale Price must follow this format: 1.50", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else if(ChooseSeller_cbx.Text == "") //If no seller is selected
            {
                MessageBox.Show("You must select a payer.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else//if they enter good data, then calculate the sale
            {
                int tempCowSales = int.Parse(CowsToSell_txt.Text);
                if (tempCowSales > totalHerd)
                {
                    MessageBox.Show("You are trying to sell more cows than you own!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    totalHerd = totalHerd - tempCowSales;
                    totalHerdNum_lbl.Text = totalHerd.ToString();

                    decimal tempTotalMade = decimal.Parse(SalePrice_txt.Text);//get total sale revenue from user's textbox

                    String tempSeller = ChooseSeller_cbx.Text; // select the seller

                    decimal hankShare = 0.00m;
                    decimal markShare = 0.00m;
                    decimal billShare = 0.00m;
                    if (hankHerd > 0)//Use these if statements to avoid the potential div by zero error
                    {
                        hankShare = tempTotalMade * hankHerd;
                    }
                    if (markHerd > 0)
                    {
                        markShare = tempTotalMade * markHerd;
                    }
                    if (billHerd > 0)
                    {
                        billShare = tempTotalMade * billHerd;
                    }

                    if (tempSeller == "Hank")        //If Hank is seller
                    {
                        hankOwes = hankOwes + markShare + billShare;
                        HankOwes_lbl.Text = hankOwes.ToString("n2");//set the global var then set labl with the var
                        hankProfit = hankProfit + tempTotalMade;
                        HankProfit_lbl.Text = hankProfit.ToString("n2");

                        owedToMark = owedToMark + markShare;
                        owedToBill = owedToBill + billShare;
                        OwedToMark_lbl.Text = owedToMark.ToString("n2");
                        OwedToBill_lbl.Text = owedToBill.ToString("n2");
                    }
                    else if (tempSeller == "Mark")  // If Mark is seller
                    {
                        markOwes = markOwes + hankShare + billShare;
                        MarkOwes_lbl.Text = markOwes.ToString("n2");
                        markProfit = markProfit + tempTotalMade;
                        MarkProfit_lbl.Text = markProfit.ToString("n2");

                        owedToHank = owedToHank + hankShare;
                        owedToBill = owedToBill + billShare;
                        OwedToHank_lbl.Text = owedToHank.ToString("n2");
                        OwedToBill_lbl.Text = owedToBill.ToString("n2");
                    }
                    else if (tempSeller == "Bill")  //If Bill is seller
                    {
                        billOwes = billOwes + hankShare + markShare;
                        BillOwes_lbl.Text = billOwes.ToString("n2");
                        billProfit = billProfit + tempTotalMade;
                        BillProfit_lbl.Text = billProfit.ToString("n2");

                        owedToHank = owedToHank + hankShare;
                        owedToMark = owedToMark + markShare;
                        OwedToHank_lbl.Text = owedToHank.ToString("n2");
                        OwedToMark_lbl.Text = owedToMark.ToString("n2");
                    }

                    totalHerdNum_lbl.Text = totalHerd.ToString();

                    //clear form
                    CowsToSell_txt.Text = "0";

                    //Solution to the Buy-Sell(orDie)-Buy Problem=======================
                    if (totalHerd < 1)//All cows are gone
                    {
                        hankHerd = 0.00m;
                        markHerd = 0.00m;
                        billHerd = 0.00m;
                        for (int j = 0; j < 25; j++)
                        {
                            HanksPercents[j] = 0.00m;
                            MarksPercents[j] = 0.00m;
                            BillsPercents[j] = 0.00m;
                        }
                        hankHerd_lbl.Text = "0.00";
                        markHerd_lbl.Text = "0.00";
                        billHerd_lbl.Text = "0.00";
                        CowPurchasesMade = 0;
                    }
                    else
                    {
                        //clear the array
                        //store the ((currentPercent * 0.01) * totalHerd)
                        //in index 0
                        for (int j = 0; j < 25; j++)
                        {
                            HanksPercents[j] = 0.00m;
                            MarksPercents[j] = 0.00m;
                            BillsPercents[j] = 0.00m;
                        }
                        HanksPercents[0] = (hankHerd * totalHerd);
                        MarksPercents[0] = (markHerd * totalHerd);
                        BillsPercents[0] = (billHerd * totalHerd);

                        CowPurchasesMade = 1;
                    }

                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void feedPaymentBtn_Click(object sender, EventArgs e)//FEE PAYMENT (not feed payment)
        {
            decimal paymentTemp = 0.00m;
            if ((decimal.TryParse(payBillText.Text, out paymentTemp) == false || decimal.Parse(payBillText.Text) <= 0))//if they enter non numeric or negative data
            {
                //display an error message and reprompt them for new data
                if (payBillText.Text == "")
                {
                    MessageBox.Show("You must enter a value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (decimal.TryParse(payBillText.Text, out paymentTemp) == false)//non numeric
                {
                    MessageBox.Show("You must enter a numeric value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (decimal.Parse(payBillText.Text) <= 0) //if < 0
                {
                    MessageBox.Show("You must enter a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else//if they enter good data, then calculate the sale
            {
                paymentTemp = decimal.Parse(payBillText.Text);
                if(FeePayer_cbx.Text == "Hank")
                {
                    if(paymentTemp > (hankOwes + 0.01m))// don't let them pay more than 1 cent over what they owe
                    {
                        MessageBox.Show("You are trying to pay more than you owe!.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        communalFund += paymentTemp;
                        comFund_lbl.Text = communalFund.ToString("n2");
                        hankOwes -= paymentTemp;
                        HankOwes_lbl.Text = hankOwes.ToString("n2");
                        payBillText.Text = "";
                        hankProfit -= paymentTemp;
                        HankProfit_lbl.Text = hankProfit.ToString("n2");
                    }
                }
                else if(FeePayer_cbx.Text == "Mark")
                {
                    if (paymentTemp > (markOwes + 0.01m))
                    {
                        MessageBox.Show("You are trying to pay more than you owe!.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        communalFund += paymentTemp;
                        comFund_lbl.Text = communalFund.ToString("n2");
                        markOwes -= paymentTemp;
                        MarkOwes_lbl.Text = markOwes.ToString("n2");
                        payBillText.Text = "";
                        markProfit -= paymentTemp;
                        MarkProfit_lbl.Text = markProfit.ToString("n2");
                    }
                }
                else if(FeePayer_cbx.Text == "Bill")
                {
                    if (paymentTemp > (billOwes + 0.01m))
                    {
                        MessageBox.Show("You are trying to pay more than you owe!.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        communalFund += paymentTemp;
                        comFund_lbl.Text = communalFund.ToString("n2");
                        billOwes -= paymentTemp;
                        BillOwes_lbl.Text = billOwes.ToString("n2");
                        payBillText.Text = "";
                        billProfit -= paymentTemp;
                        BillProfit_lbl.Text = billProfit.ToString("n2");
                    }
                }
                else
                {
                    MessageBox.Show("You must select a payer.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void recieveEarningsBtn_Click(object sender, EventArgs e)
        {
            decimal recieveTemp = 0.00m;
            if ((decimal.TryParse(recievedEarningsTxt.Text, out recieveTemp) == false || decimal.Parse(recievedEarningsTxt.Text) <= 0))//if they enter non numeric or negative data
            {
                //display an error message and reprompt them for new data
                if (recievedEarningsTxt.Text == "")
                {
                    MessageBox.Show("You must enter a value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (decimal.TryParse(recievedEarningsTxt.Text, out recieveTemp) == false)//non numeric
                {
                    MessageBox.Show("You must enter a numeric value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (decimal.Parse(recievedEarningsTxt.Text) <= 0) //if < 0
                {
                    MessageBox.Show("You must enter a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else//if they enter good data, then calculate the sale
            {
                if (Reciever_cbx.Text == "Hank")
                {
                    if (recieveTemp > communalFund)
                    {
                        MessageBox.Show("Value exceeds amount in shared Account!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (recieveTemp > (owedToHank + 0.01m))
                    {
                        MessageBox.Show("You are trying to extract more than you are owed!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        communalFund -= recieveTemp;
                        comFund_lbl.Text = communalFund.ToString("n2");
                        owedToHank -= recieveTemp;
                        OwedToHank_lbl.Text = owedToHank.ToString("n2");
                        hankProfit += recieveTemp;
                        HankProfit_lbl.Text = hankProfit.ToString("n2");
                    }
                }
                else if (Reciever_cbx.Text == "Mark")
                {
                    if (recieveTemp > communalFund)
                    {
                        MessageBox.Show("Value exceeds amount in shared Account!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (recieveTemp > (owedToMark + 0.01m))
                    {
                        MessageBox.Show("You are trying to extract more than you are owed!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        communalFund -= recieveTemp;
                        comFund_lbl.Text = communalFund.ToString("n2");
                        owedToMark -= recieveTemp;
                        OwedToMark_lbl.Text = owedToMark.ToString("n2");
                        markProfit += recieveTemp;
                        MarkProfit_lbl.Text = markProfit.ToString("n2");
                    }
                }
                else if (Reciever_cbx.Text == "Bill")
                {
                    if (recieveTemp > communalFund)
                    {
                        MessageBox.Show("Value exceeds amount in shared Account!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (recieveTemp > (owedToBill + 0.01m))
                    {
                        MessageBox.Show("You are trying to extract more than you are owed!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        communalFund -= recieveTemp;
                        comFund_lbl.Text = communalFund.ToString("n2");
                        owedToBill -= recieveTemp;
                        OwedToBill_lbl.Text = owedToBill.ToString("n2");
                        billProfit += recieveTemp;
                        BillProfit_lbl.Text = billProfit.ToString("n2");
                    }
                }
                else
                {
                    MessageBox.Show("You must select someone to reimburse.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void recievedEarningsTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void Death_btn_Click(object sender, EventArgs e)// Report DEATH(S)
        {
            //Error Checking Death Box
            int numDead = 0;
            if (NumDead_txt.Text == "")
            {
                MessageBox.Show("You must enter a value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (int.TryParse(NumDead_txt.Text, out numDead) == false)//non numeric
            {
                MessageBox.Show("You must enter an integer value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (int.Parse(NumDead_txt.Text) <= 0) //if < 0
            {
                MessageBox.Show("You must enter a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (int.Parse(NumDead_txt.Text) > totalHerd)// more deaths than cows
            {
                MessageBox.Show("You are trying to report more deaths than there are cows.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                numDead = int.Parse(NumDead_txt.Text);
                totalHerd -= numDead;
                totalHerdNum_lbl.Text = totalHerd.ToString();
                NumDead_txt.Text = "";

                //Solution to the Buy-Sell-Buy Problem=======================
                if(totalHerd < 1)//All cows are gone
                {
                    hankHerd = 0.00m;
                    markHerd = 0.00m;
                    billHerd = 0.00m;
                    for(int j = 0; j < 25; j++)
                    {
                        HanksPercents[j] = 0.00m;
                        MarksPercents[j] = 0.00m;
                        BillsPercents[j] = 0.00m;
                    }
                    hankHerd_lbl.Text = "0.00";
                    markHerd_lbl.Text = "0.00";
                    billHerd_lbl.Text = "0.00";
                    CowPurchasesMade = 0;
                }
                else
                {
                    //clear the array
                    //store the ((currentPercent * 0.01) * totalHerd)
                    //in index 0
                    for (int j = 0; j < 25; j++)
                    {
                        HanksPercents[j] = 0.00m;
                        MarksPercents[j] = 0.00m;
                        BillsPercents[j] = 0.00m;
                    }
                    HanksPercents[0] = (hankHerd * totalHerd);
                    MarksPercents[0] = (markHerd * totalHerd);
                    BillsPercents[0] = (billHerd * totalHerd);

                    CowPurchasesMade = 1;
                }

            }
        }


        private void ConfirmBuyCows_btn_Click(object sender, EventArgs e)
        {
            //ERROR CHECK EACH BOX AND MAKE SURE THE PERCENT ADDS UP TO 100%?
            int cowsBought = 0;
            decimal purchaseCost = 0.00m;
            decimal thisPercentTotal = 0.00m;
            decimal fieldTester = 0.00m;

            if (CowsPurchased_txt.Text == "") //Error Check Cows Sold TextBox
            {
                MessageBox.Show("You must enter a value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (int.TryParse(CowsPurchased_txt.Text, out cowsBought) == false)//non numeric
            {
                MessageBox.Show("You must enter a numeric value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (int.Parse(CowsPurchased_txt.Text) <= 0.00) //if < 0
            {
                MessageBox.Show("You must enter a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (CowPurchasesMade >= 25)//More than 25 purchases attempted in a season - error check
            {
                MessageBox.Show("You cannot make more than 25 cattle purchases per season.", "Bounds Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //Error Check Hank's Percent ===============
            else if (decimal.TryParse(HankCowsPurchased_txt.Text, out fieldTester) == false)//non numeric
            {
                MessageBox.Show("Hank's percent of the purchase must be a numeric value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (decimal.Parse(HankCowsPurchased_txt.Text) < 0.00m) //if < 0
            {
                MessageBox.Show("Hank's percent of the purchase a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Error Check Mark's Percent ===============
            else if (decimal.TryParse(MarkCowsPurchased_txt.Text, out fieldTester) == false)//non numeric
            {
                MessageBox.Show("Mark's percent of the purchase must be a numeric value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (decimal.Parse(MarkCowsPurchased_txt.Text) < 0.00m) //if < 0
            {
                MessageBox.Show("Mark's percent of the purchase a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Error Check Bill's Percent ===============
            else if (decimal.TryParse(BillCowsPurchased_txt.Text, out fieldTester) == false)//non numeric
            {
                MessageBox.Show("Bill's percent of the purchase must be a numeric value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (decimal.Parse(BillCowsPurchased_txt.Text) < 0.00m) //if < 0
            {
                MessageBox.Show("Bill's percent of the purchase a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (CowBuyer_cbx.Text == "") //Error Check if Buyer cbx is empty
            {
                MessageBox.Show("You must select a payer.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (CostOfCowPurchase_txt.Text == "") //Error Check Cost of Purchase TextBox
            {
                MessageBox.Show("You must enter a value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (decimal.TryParse(CostOfCowPurchase_txt.Text, out purchaseCost) == false)//non numeric
            {
                MessageBox.Show("You must enter a numeric value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (decimal.Parse(CostOfCowPurchase_txt.Text) <= 0.00m) //if < 0
            {
                MessageBox.Show("You must enter a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else      //Good Data - Do Calculations===============================================================
            {

                thisPercentTotal = decimal.Parse(HankCowsPurchased_txt.Text) + decimal.Parse(MarkCowsPurchased_txt.Text) + decimal.Parse(BillCowsPurchased_txt.Text);
                if (thisPercentTotal < 99.0m || thisPercentTotal > 100.0m)//if percents don't add up to ~100%
                {
                    MessageBox.Show("Your percents should add up to approximately 100%", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else//GOOD DATA
                {
                    cowsBought = int.Parse(CowsPurchased_txt.Text);
                    totalHerd += cowsBought;
                    totalHerdNum_lbl.Text = totalHerd.ToString();

                    //Get each partner's percent of this new group of cows
                    decimal hanksPercentThisGroup = 0.00m;
                    decimal marksPercentThisGroup = 0.00m;
                    decimal billsPercentThisGroup = 0.00m;

                    hanksPercentThisGroup = decimal.Parse(HankCowsPurchased_txt.Text) * 0.01m;// * 0.01m; is to Convert to decimal form
                    marksPercentThisGroup = decimal.Parse(MarkCowsPurchased_txt.Text) * 0.01m;
                    billsPercentThisGroup = decimal.Parse(BillCowsPurchased_txt.Text) * 0.01m;

                    //Store these percents in the current index of the percents arrays
                    HanksPercents[CowPurchasesMade] = hanksPercentThisGroup * cowsBought;//store in array the percent in
                    MarksPercents[CowPurchasesMade] = marksPercentThisGroup * cowsBought;//his box times cows bought this time
                    BillsPercents[CowPurchasesMade] = billsPercentThisGroup * cowsBought;//to get total percent add all his array and divide by total Num Of Cows

                    //increment cow purchases made (index)
                    CowPurchasesMade += 1;

                    //Calculate each partner's new Ownership % of total Herd
                    //add all the indexes then divide by CowPurchasesMade
                    hankHerd = 0;
                    markHerd = 0;
                    billHerd = 0;
                    for (int counter = 0; counter < 25; counter++)
                    {
                        hankHerd += HanksPercents[counter];
                        markHerd += MarksPercents[counter];
                        billHerd += BillsPercents[counter];
                    }

                    hankHerd = hankHerd / totalHerd;
                    markHerd = markHerd / totalHerd;
                    billHerd = billHerd / totalHerd;

                    //set percents owned labels
                    hankHerd_lbl.Text = (hankHerd * 100).ToString("n4");//Multiply by 100 to display percent form
                    markHerd_lbl.Text = (markHerd * 100).ToString("n4");
                    billHerd_lbl.Text = (billHerd * 100).ToString("n4");

                    //get purchase cost
                    purchaseCost = decimal.Parse(CostOfCowPurchase_txt.Text);

                    //DETERMINE WHO MADE THE PURCHASE THEN HAVE A CORRESPONDING IF STATEMENT FOR EACH PARTNER
                    // take entire cost of purchase from Hank, then Mark and Bill owe him
                    //then mark and bill pay him and he eats the cost of his share, only making it back through sales
                    decimal hankCowCost = 0.00m;
                    decimal markCowCost = 0.00m;
                    decimal billCowCost = 0.00m;
                    if (CowBuyer_cbx.Text == "Hank")
                    {
                        hankProfit -= purchaseCost;
                        HankProfit_lbl.Text = hankProfit.ToString("n2");

                        hankCowCost = purchaseCost * hankHerd;// * since  hankHerd is 0.33 (prevents "/ 0" errors)
                        owedToHank += (purchaseCost - hankCowCost);
                        OwedToHank_lbl.Text = owedToHank.ToString("n2");

                        markCowCost = purchaseCost * markHerd;
                        markOwes += markCowCost;
                        MarkOwes_lbl.Text = markOwes.ToString("n2");

                        billCowCost = purchaseCost * billHerd;
                        billOwes += billCowCost;
                        BillOwes_lbl.Text = billOwes.ToString("n2");
                    }
                    else if (CowBuyer_cbx.Text == "Mark")
                    {
                        markProfit -= purchaseCost;
                        MarkProfit_lbl.Text = markProfit.ToString("n2");

                        markCowCost = purchaseCost * markHerd;
                        owedToMark += (purchaseCost - markCowCost);
                        OwedToMark_lbl.Text = owedToMark.ToString("n2");

                        hankCowCost = purchaseCost * hankHerd;
                        hankOwes += hankCowCost;
                        HankOwes_lbl.Text = hankOwes.ToString("n2");

                        billCowCost = purchaseCost * billHerd;
                        billOwes += billCowCost;
                        BillOwes_lbl.Text = billOwes.ToString("n2");
                    }
                    else if (CowBuyer_cbx.Text == "Bill")
                    {
                        billProfit -= purchaseCost;
                        BillProfit_lbl.Text = billProfit.ToString("n2");

                        billCowCost = purchaseCost * billHerd;
                        owedToBill += (purchaseCost - billCowCost);
                        OwedToBill_lbl.Text = owedToBill.ToString("n2");

                        hankCowCost = purchaseCost * hankHerd;
                        hankOwes += hankCowCost;
                        HankOwes_lbl.Text = hankOwes.ToString("n2");

                        markCowCost = purchaseCost * markHerd;
                        markOwes += markCowCost;
                        MarkOwes_lbl.Text = markOwes.ToString("n2");
                    }
                    else // No buyer selected
                    {
                        MessageBox.Show("You must select a buyer!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void BuyFeed_btn_Click(object sender, EventArgs e) //BUY FEED
        {
            decimal foodCost = 0.00m;
            String buyer = "";
            //Error Check CostOfFood_txt
            if (decimal.TryParse(CostOfFeed_txt.Text, out foodCost) == false)//non numeric
            {
                MessageBox.Show("You must enter a numeric value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (decimal.Parse(CostOfFeed_txt.Text) <= 0.00m) //if < 0
            {
                MessageBox.Show("You must enter a positive value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Error Check FeedBuyer_cbx
            else if (FeedBuyer_cbx.Text == "") //if < 0
            {
                MessageBox.Show("You must select a buyer!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Else data is good -> Proceed with Calculations
            else
            {
                decimal tempHankBill = 0.00m;
                decimal tempmarkBill = 0.00m;
                decimal tempbillBill = 0.00m;
                foodCost = decimal.Parse(CostOfFeed_txt.Text);
                decimal hankFoodCost = 0.00m;
                decimal markFoodCost = 0.00m;
                decimal billFoodCost = 0.00m;
                buyer = FeedBuyer_cbx.Text;

                if(buyer == "Hank")
                {
                    hankProfit -= foodCost;
                    HankProfit_lbl.Text = hankProfit.ToString("n2");

                    hankFoodCost = foodCost * hankHerd;// * since  hankHerd is 0.33 (prevents "/ 0" errors)
                    owedToHank += (foodCost - hankFoodCost);
                    OwedToHank_lbl.Text = owedToHank.ToString("n2");

                    markFoodCost = foodCost * markHerd;
                    markOwes += markFoodCost;
                    MarkOwes_lbl.Text = markOwes.ToString("n2");

                    billFoodCost = foodCost * billHerd;
                    billOwes += billFoodCost;
                    BillOwes_lbl.Text = billOwes.ToString("n2");

                }
                else if(buyer == "Mark")
                {
                    markProfit -= foodCost;
                    MarkProfit_lbl.Text = markProfit.ToString("n2");

                    markFoodCost = foodCost * markHerd;
                    owedToMark += (foodCost - markFoodCost);
                    OwedToMark_lbl.Text = owedToMark.ToString("n2");

                    hankFoodCost = foodCost * hankHerd;
                    hankOwes += hankFoodCost;
                    HankOwes_lbl.Text = hankOwes.ToString("n2");

                    billFoodCost = foodCost * billHerd;
                    billOwes += billFoodCost;
                    BillOwes_lbl.Text = billOwes.ToString("n2");
                }
                else if(buyer == "Bill")
                {
                    billProfit -= foodCost;
                    BillProfit_lbl.Text = billProfit.ToString("n2");

                    billFoodCost = foodCost * billHerd;
                    owedToBill += (foodCost - billFoodCost);
                    OwedToBill_lbl.Text = owedToBill.ToString("n2");

                    hankFoodCost = foodCost * hankHerd;
                    hankOwes += hankFoodCost;
                    HankOwes_lbl.Text = hankOwes.ToString("n2");

                    markFoodCost = foodCost * markHerd;
                    markOwes += markFoodCost;
                    MarkOwes_lbl.Text = markOwes.ToString("n2");
                }
                else//Just in case
                {
                    MessageBox.Show("You must select a buyer!", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //file save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)//Writes Data to a File
        {
            StreamWriter saver;
            try
            {
                saver = new StreamWriter(filenm);

                saver.WriteLine(HankOwes_lbl.Text);//Save first row of stats table
                saver.WriteLine("");
                saver.WriteLine(MarkOwes_lbl.Text);
                saver.WriteLine("");
                saver.WriteLine(BillOwes_lbl.Text);
                saver.WriteLine("");

                saver.WriteLine(OwedToHank_lbl.Text);//Save second row of stats table
                saver.WriteLine("");
                saver.WriteLine(OwedToMark_lbl.Text);
                saver.WriteLine("");
                saver.WriteLine(OwedToBill_lbl.Text);
                saver.WriteLine("");

                saver.WriteLine(HankProfit_lbl.Text);//Save third row of stats table
                saver.WriteLine("");
                saver.WriteLine(MarkProfit_lbl.Text);
                saver.WriteLine("");
                saver.WriteLine(BillProfit_lbl.Text);
                saver.WriteLine("");

                saver.WriteLine(hankHerd_lbl.Text);//Save fourth row of stats table
                saver.WriteLine("");
                saver.WriteLine(markHerd_lbl.Text);
                saver.WriteLine("");
                saver.WriteLine(billHerd_lbl.Text);
                saver.WriteLine("");

                saver.WriteLine(totalHerdNum_lbl.Text);//Saving Herd Size
                saver.WriteLine("");

                saver.WriteLine(comFund_lbl.Text);//Saving Shared Account (Communal Fund)
                saver.WriteLine("");

                //Save each partners current percent
                saver.WriteLine(hankHerd.ToString());
                saver.WriteLine("");

                saver.WriteLine(markHerd.ToString());
                saver.WriteLine("");

                saver.WriteLine(billHerd.ToString());
                saver.WriteLine("");

                //Saving the OwnerShip Percentage Arrays
                saver.WriteLine(CowPurchasesMade.ToString());//Save # of Cow Purchases made so far
                saver.WriteLine("");

                for (int count = 0; count < 25; count++)//Saving Hank's Percents of cattle purchases
                {
                    saver.WriteLine(HanksPercents[count]);
                }
                saver.WriteLine("");

                for (int count = 0; count < 25; count++)//Saving Mark's Percents of cattle purchases
                {
                    saver.WriteLine(MarksPercents[count]);
                }
                saver.WriteLine("");

                for (int count = 0; count < 25; count++)//Saving Bill's Percents of cattle purchases
                {
                    saver.WriteLine(BillsPercents[count]);
                }
                saver.WriteLine("");


                saver.Close();//Closes saver when done writing

                MessageBox.Show("Your data has been saved.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Your data was not saved because of an error. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        //File Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Close Form (Warning Message)
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult yesNo;
            yesNo = MessageBox.Show("Exiting will cause any unsaved changes to be lost. Are you sure you are ready to exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(yesNo == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void NewSeason_btn_Click(object sender, EventArgs e) //START A NEW SEASON
        {
            DialogResult yesNo;
            yesNo = MessageBox.Show("Are you sure you want to begin a new season? This will cause all previously saved data to be erased! THIS ACTION IS NOT REVERSIBLE!", "New Season", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (yesNo == DialogResult.No)
            {
                //e.Cancel = true;
            }
            else//start new season -- same as saver but use all zeros
            {
                StreamWriter saver;
                try
                {
                    saver = new StreamWriter(filenm);

                    saver.WriteLine("0.00");//Save first row of stats table
                    saver.WriteLine("");
                    saver.WriteLine("0.00");
                    saver.WriteLine("");
                    saver.WriteLine("0.00");
                    saver.WriteLine("");

                    saver.WriteLine("0.00");//Save second row of stats table
                    saver.WriteLine("");
                    saver.WriteLine("0.00");
                    saver.WriteLine("");
                    saver.WriteLine("0.00");
                    saver.WriteLine("");

                    saver.WriteLine("0.00");//Save third row of stats table
                    saver.WriteLine("");
                    saver.WriteLine("0.00");
                    saver.WriteLine("");
                    saver.WriteLine("0.00");
                    saver.WriteLine("");

                    saver.WriteLine("0");//Save fourth row of stats table
                    saver.WriteLine("");
                    saver.WriteLine("0");
                    saver.WriteLine("");
                    saver.WriteLine("0");
                    saver.WriteLine("");

                    saver.WriteLine("0");//Saving Herd Size
                    saver.WriteLine("");

                    saver.WriteLine("0.00");//Saving Shared Account (Communal Fund)
                    saver.WriteLine("");

                    //Save each partners current percent
                    saver.WriteLine("0.00");
                    saver.WriteLine("");

                    saver.WriteLine("0.00");
                    saver.WriteLine("");

                    saver.WriteLine("0.00");
                    saver.WriteLine("");

                    //Saving the OwnerShip Percentage Arrays
                    saver.WriteLine("0");//Save # of Cow Purchases made so far
                    saver.WriteLine("");

                    for (int count = 0; count < 25; count++)//Saving Hank's Percents of cattle purchases
                    {
                        saver.WriteLine("0");
                    }
                    saver.WriteLine("");

                    for (int count = 0; count < 25; count++)//Saving Mark's Percents of cattle purchases
                    {
                        saver.WriteLine("0");
                    }
                    saver.WriteLine("");

                    for (int count = 0; count < 25; count++)//Saving Bill's Percents of cattle purchases
                    {
                        saver.WriteLine("0");
                    }
                    saver.WriteLine("");


                    saver.Close();//Closes saver when done writing

                    MessageBox.Show("You have successfully started a new season.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("A new season could not be generated. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


                //Clear and Zero the fields
                SalePrice_txt.Text = "";
                CowsToSell_txt.Text = "";

                CowsPurchased_txt.Text = "";
                HankCowsPurchased_txt.Text = "";
                MarkCowsPurchased_txt.Text = "";
                BillCowsPurchased_txt.Text = "";
                CostOfCowPurchase_txt.Text = "";

                CostOfFeed_txt.Text = "";

                NumDead_txt.Text = "";

                payBillText.Text = "";

                recievedEarningsTxt.Text = "";

                totalHerdNum_lbl.Text = "0";
                comFund_lbl.Text = "0.00";

                HankOwes_lbl.Text = "0.00";
                MarkOwes_lbl.Text = "0.00";
                BillOwes_lbl.Text = "0.00";

                OwedToHank_lbl.Text = "0.00";
                OwedToMark_lbl.Text = "0.00";
                OwedToBill_lbl.Text = "0.00";

                HankProfit_lbl.Text = "0.00";
                MarkProfit_lbl.Text = "0.00";
                BillProfit_lbl.Text = "0.00";

                hankHerd_lbl.Text = "0";
                markHerd_lbl.Text = "0";
                billHerd_lbl.Text = "0";

                //CLEAR PERCENT ARRAYS
                for (int count = 0; count < 25; count++)
                {
                    HanksPercents[count] = 0.00m;
                    MarksPercents[count] = 0.00m;
                    BillsPercents[count] = 0.00m;
                }

                //Reset the global variables

                hankHerd = 0.00m;
                markHerd = 0.00m;
                billHerd = 0.00m;
                totalHerd = 0;

                hankOwes = 0.00m;
                markOwes = 0.00m;
                billOwes = 0.00m;

                owedToHank = 0.00m;
                owedToMark = 0.00m;
                owedToBill = 0.00m;

                hankProfit = 0.00m;
                markProfit = 0.00m;
                billProfit = 0.00m;

                CowPurchasesMade = 0;

                communalFund = 0.00m;

            }
        }

        private void changeThemeToolStripMenuItem_Click(object sender, EventArgs e)//Dark Mode / Light Mode
        {
            if(ThemeSelected == false)//switch to dark theme
            {
                ThemeSelected = true;
                this.BackColor = Color.Black;
                SellCows_grp.BackColor = Color.CornflowerBlue;
                groupBox1.BackColor = Color.Gray;
                groupBox2.BackColor = Color.CornflowerBlue;
                groupBox3.BackColor = Color.Gray;
                groupBox6.BackColor = Color.CornflowerBlue;
                groupBox4.BackColor = Color.SpringGreen;
                groupBox5.BackColor = Color.LimeGreen;
            }
            else//set to light theme
            {
                ThemeSelected = false;
                this.BackColor = Color.Empty;
                SellCows_grp.BackColor = Color.LightSkyBlue;
                groupBox1.BackColor = Color.Silver;
                groupBox2.BackColor = Color.LightSkyBlue;
                groupBox3.BackColor = Color.Silver;
                groupBox6.BackColor = Color.LightSkyBlue;
                groupBox4.BackColor = Color.PaleGreen;
                groupBox5.BackColor = Color.MediumSpringGreen;
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}