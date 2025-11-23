import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom';
import { KTCard } from '../../../_metronic/helpers';
import { Content } from '../../../_metronic/layout/components/content';
import CustomTable from '../../../_shared/CustomTable/Index';
import FarmerService from '../../../services/FarmerService';
import moment from 'moment';
import config from '../../../environments/config';
import KycService from '../../../services/KycService';

const farmerService = new FarmerService();
const kycService = new KycService();

const PhoneVerificationStatusReport = () => {
  const [rowData, setRowData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const StatusComponent = (props: any) => {
    return <div className="py-1">
      {props.data.isFarmerVerified === true && <div className='badge fs-7 badge-light-success  fw-normal'>Yes</div>}
      {props.data.isFarmerVerified === false && <div className='badge fs-7 badge-light-danger  fw-normal'>No</div>}
    </div>;
  };

  const farmerComponent = (props: any) => {
    const farmer = props.data;
    const fields = props.fields || []; // Fields to display

    if (!farmer) {
      return <div>No Farmer Data</div>;
    }

    return (
      <div className="d-flex flex-column">
        {fields.map((field: string) => (
          <div key={field}>{farmer[field] || "Not Available"}</div>
        ))}
      </div>
    );
  };

  const [colDefs] = useState<any>([
    {
      field: "fullName",
      flex: 1,
    },
    { field: "systemId", flex: 1, headerName: "System Id" },
    {
      field: "mobile",
      flex: 1,
    },

    { field: "isFarmerVerified", headerName: "Is Mobile Verified", flex: 1, cellRenderer: StatusComponent },
    {
      field: "mobileLastVerifiedOn", headerName: "Last Verified On", flex: 1,
      valueFormatter: function (params: any) {
        return params.value != null ? moment(params.value).format(config.dateOnlyFormat) : "";
      },
    },
  ]);

  const bindData = async () => {
    const searchParams = {};
    const response = await farmerService.getFarmerData(searchParams);
    
    setRowData(response.farmers);
  }

  const revalidateMobileNumbers = async () => {
    setLoading(true);
    const response = await kycService.revalidateMobileNumbers();

    setTimeout(() => {
      bindData();
      setLoading(false);
    }, 1000)
  }

  useEffect(() => {
    bindData();
  }, []);

  return (
    <Content>
      <div className='d-flex justify-content-end' data-kt-user-table-toolbar='base'>
        <div className="m-3">
          <button type='button' className='btn btn-primary me-3' onClick={() => revalidateMobileNumbers()}>

            {!loading && <span className='indicator-label'>Revalidate</span>}
            {loading && (
              <span className='indicator-progress' style={{ display: 'block' }}>
                Please wait...
                <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
              </span>
            )}
          </button>
        </div>
      </div>
      <KTCard>

        <CustomTable
          rowData={rowData}
          colDefs={colDefs}
        />
      </KTCard>
    </Content>
  )
}


export default PhoneVerificationStatusReport