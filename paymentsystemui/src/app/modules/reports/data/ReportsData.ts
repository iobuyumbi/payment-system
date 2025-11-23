export const reportSetupData = [
  // {
  //   id: 1,
  //   name: "General Reports",
  //   link: '/reports/loan-account-summary',
  //   reports: [{ link: "/reports/loan-account-summary", name: "Loan Accounts" },
  //   { link: "", name: "Loan Products" },
  //   { link: "", name: "Repayments" },
  //   { link: "", name: "Non-performing Loans" }]
  // },
  // {
  //   id: 2,
  //   name: "Country Level Reports",
  //   reports:[{link:"", name : "loren ipsum"} ,
  //       {link:"", name :"loren ipsum"},
  //       {link:"",name : "loren ipsum"},
  //       {link:"",name :"loren ipsum"}]
  // },
  // {
  //   id: 3,
  //   name: "Project Level Reports",
  //   reports:[{link:"", name : " loren ipsum"} ,
  //       {link:"", name :"loren ipsum"},
  //       {link:"",name : "loren ipsum"},
  //       {link:"",name :"loren ipsum"}]
  // },
  {
    id: 4,
    name: "Loan Portfolio Reports",
    link: '/reports/loan-portfolio-summary',
    reports: [{ link: "", name: "Loan Applications" },
    { link: "", name: "Repayments" },
    { link: "", name: "Non-performing Loans" },
    { link: "", name: "Accounts Summary" }]
  },
  {
    id: 5,
    name: "Loan Account Reports",
    link: '/reports/loan-performance-summary',
    reports: [{ link: "", name: "Loan Items Summary" },
    { link: "", name: "Loan Balances" },
    { link: "", name: "Loan Performance" }
    ]
  },
  {
    id: 6,
    name: "Payment Batch Reports",
    link: '/reports/payment-batch-summary',
    reports: [{ link: "/reports/confirmation-report", name: " Payment Confirmation Status" },
    { link: "/reports/phone-verification-report", name: "Phone Verification Status" },
    { link: "/reports/repayment-report", name: "Loan Repayment Deductions" },
    { link: "/reports/transaction-report", name: "Transactions Report" }]
  },
];
