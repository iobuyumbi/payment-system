import React, { useEffect } from 'react'
import LocationService from '../../../services/LocationService';
import CustomTable from '../../../_shared/CustomTable/Index';
import { Content } from '../../../_metronic/layout/components/content';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { KTIcon } from '../../../_metronic/helpers';
import { useNavigate } from 'react-router-dom';
const breadCrumbs: Array<PageLink> = [
  {
    title: "Dashboard",
    path: "/dashboard",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: true,
  },
];

const locationServices = new LocationService();


const ListLocations = () => {
  const [rowData, setRowData] = React.useState<any[]>([]);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [error, setError] = React.useState<string | null>(null);
  const [searchTerm, setSearchTerm] = React.useState<string>('');
 const navigate = useNavigate();

   const editButtonHandler = (value: any) => {
    
      navigate(`/locations/edit/${value.id}`);
    };
  

  const fetchLocations = async () => {
    setLoading(true);
    try {
      // Simulate an API call
      
      const data: any = {
        pageNumber: 1,
        pageSize: 10000,
        filter: "",
      };
      const response = await locationServices.getLocations(data); // Replace with actual API endpoint
      if (!response) {
        throw new Error('Failed to fetch locations');
      }
     console.log(response);
      setRowData(response);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    fetchLocations();
  }, []);
 const CustomActionComponent = (props: any) => {
    return (
      <div className="d-flex flex-row">
       
        {/* Edit */}
        {isAllowed("farmers.edit") && (
          <button
            className="btn btn-default mx-0 px-1"
            onClick={() => editButtonHandler(props.data)}
          >
            <KTIcon
              iconName="pencil"
              iconType="outline"
              className="fs-4 text-gray-600"
            />
          </button>
        )}

        {/* Delete
        {isAllowed("farmers.delete") && (
          <button
            className="btn btn-default mx-0 px-1"
            onClick={() => openDeleteModal(props.data.id)}
          >
            <KTIcon
              iconName="trash"
              iconType="outline"
              className="text-danger fs-4"
            />
          </button>
        )} */}
      </div>
    );
  };
  const [colDefs, setColDefs] = React.useState<any>([
   
    
    { headerName: "Address line 1", field: "addressLine1", flex: 1 },
    { headerName: "Address line 2", field: "addressLine2", flex: 1 },
    { headerName: "Phone Number", field: "phoneNumber", flex: 1 },
    { headerName: "Email", field: "supportEmail", flex: 1 },
    { headerName: "City", field: "city", flex: 1 },
    { headerName: "State", field: "state", flex: 1 },
    // { headerName: "Country", field: "country", flex: 1 },
    { headerName: "Postal Code", field: "zipCode", flex: 1 },
        { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);
  
 


  return (
      <Content>
    <div>
       <PageTitleWrapper />
          <PageTitle breadcrumbs={breadCrumbs}>Locations</PageTitle>
      <CustomTable 
      rowData={rowData}
       colDefs={colDefs} 
       header="Locations"
      addBtnText={ "Add location" } 
        addBtnLink={ "/locations/add" }
        />
    </div>
    </Content>
  )
}

export default ListLocations
