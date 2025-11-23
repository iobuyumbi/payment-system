import { useState, useEffect } from 'react';
import { Content } from '../../../../_metronic/layout/components/content'
import CustomTable from '../../../../_shared/CustomTable/Index';
import LoanApplicationService from '../../../../services/LoanApplicationService';
import moment from 'moment';
import config from '../../../../environments/config';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { addToLoanApplication } from '../../../../_features/loan/loanApplicationSlice';
import { useDispatch } from 'react-redux';
import { AppStatusBadge } from '../../../../_shared/Status/AppStatusBadge';

const loanApplicationService = new LoanApplicationService();

interface Loan {
  loanId: string;
  borrowerName: string;
  amount: number;
  interestRate: number;
  startDate: string;
  endDate: string;
  status: string;
}

// Define the component props
interface ActiveLoansProps {
  userId: string;
}

const FarmerLoans = (props : any) => {
   const { farmer } = props;
 const navigate = useNavigate();
  const dispatch = useDispatch();
  const [rowData, setRowData] = useState<any>();
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedId, setSetSelectedId] = useState<any>(null);
 
  const [loanData, setLoanData] = useState<any>(null);

  const linkButtonHandler = (value: any) => {
    dispatch(addToLoanApplication(value))
    navigate('/loans/applications/detail')
  }
  // Fetch active loans for the user
  useEffect(() => {
    const fetchLoans = async () => {
      try {
        setLoading(true);
        const response = await loanApplicationService.getFarmerLoanApplications(farmer.id);
        console.log("Farmer Loans Response", response);
        setRowData(response);
      } catch (err) {
        setError("Failed to fetch loans. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchLoans();
  }, [farmer.id]);

  const ActionComponent = (props: any) => {
    debugger
    return <div className="d-flex flex-row">
      {/* <a className="link-primary" onClick={() => showDetails(props.data.loanApplicationId)}>
        View Detail
      </a> */}
    <button className="btn btn-default link-primary mx-0 px-1" onClick={() => linkButtonHandler(props.data)}>
      {props.data.loanNumber}
    </button>;
    </div>;
  };
 const DateComponent = (props: any) => {
    return (
      <div className="d-flex flex-row">
        {moment(props.data.createdOn).format('YYYY-MM-DD HH:mm ')}
      </div>
    );
  };
 const statusRenderer = (props: any) => {
    return <AppStatusBadge value={props.data.statusText} />

  };
  // Define the table columns
  const [colDefs] = useState<any>([
    { headerName: "Loan Account No", flex: 1, cellRenderer: ActionComponent },
   {
      headerName: "Total Items Value",
      field: "totalValue", valueFormatter: (params: any) => {
        return parseFloat(params.value).toFixed(2);
      }, filter: true, flex: 1
    },
    {
      field: "feeApplied", valueFormatter: (params: any) => {
        return parseFloat(params.value).toFixed(2);
      }, filter: true, width: 140,
    },
    {
      headerName: "Effective Principal",
      field: "principalAmount", valueFormatter: (params: any) => {
        return parseFloat(params.value).toFixed(2);
      }, filter: true, width: 180,
    },
    { field: "createdOn", width: 120, filter: true, cellRenderer: DateComponent },
    { field: "status", width: 120, filter: true, cellRenderer: statusRenderer },
    // { headerName: "Status", flex: 1, cellRenderer: ActionComponent },
  ]);

  return (
    <Content>
      <div className='card shadow-none rounded-0 '>
        <div className='card-header' id='kt_activities_header'>
          <h3 className='card-title fw-bolder text-gray-900'>Farmer Loans</h3>
          <div className='card-toolbar'>
          </div>
        </div>
        <div className='p-0'>
            <CustomTable
              rowData={rowData}
              colDefs={colDefs}
              header=""
              addBtnText={""}
              height={250}
            />
        </div>

        {selectedId &&
          <div className="mx-10">
            {/* <div className="d-flex justify-content-center bg-gray-100 rounded text-center pt-4 pb-2 m-5">
              <p>Next Interest will be calculated on <strong>15 Jan 2026</strong> </p>
            </div> */}
            {/* <div>
              <div className='d-flex flex-center flex-wrap '>
                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 mx-3 mb-3'>
                  <div className='fs-4 fw-bolder text-gray-700'>{loanData?.application?.principalAmount?.toFixed(2)}</div>
                  <div className=' text-gray-500'>Loan Amount</div>
                </div>

                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 mx-3 mb-3'>
                  <div className='fs-4 fw-bolder text-gray-700'>-</div>
                  <div className=' text-gray-500'>Annual Interest</div>
                </div>

                <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 mx-3 px-4 mb-3'>
                  <div className='fs-4 fw-bolder text-gray-700'>{'-'}</div>
                  <div className=' text-gray-500'>Tenure</div>
                </div>
              </div>
            </div> */}
          </div>
        }
      </div>
    </Content>
  )
}

export default FarmerLoans
