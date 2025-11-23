import { useEffect, useState } from "react";
import moment from "moment";
import LoanApplicationService from "../../../../services/LoanApplicationService";
import { AppStatusBadge } from "../../../../_shared/Status/AppStatusBadge";

const loanApplicationService = new LoanApplicationService();

export function ApplicationHistory(props: any) {
  const { id } = props;
  const [data, setData] = useState<any>();

  const bindApplicationsStatusHistory = async (id: any) => {
    const data = {
      pageNumber: 1,
      pageSize: 10,
    };
    const response = await loanApplicationService.getApplicationStatusLog(id);
    setData(response);
  };

  useEffect(() => {
    bindApplicationsStatusHistory(id);
  }, []);

  return (
    <table className="table align-middle table-row-dashed fs-6 gy-5 dataTable no-footer">
      <thead>
        <tr className="text-start text-muted fw-bolder fs-7 text-uppercase gs-0">
          <th>Date</th>
          <th>Status </th>
          <th>Updated By</th>
          <th>Remarks</th>
        </tr>
      </thead>
      <tbody>
        {data && data.length > 0 ? (
          data.map((item: any, index: any) => (
            <tr key={index}>
              <td className="text-gray-600 fw-bold ">
                {moment(item.createdOn).format('YYYY-MM-DD hh:mm A')}
              </td>
              <td className="fw-bold ">
                {/* {item.statusId === "6f103a88-8443-45ad-9c37-afe07f6b48e1" ?  
                <span className='badge badge-light-info fs-8 fw-normal px-3 py-2 '>
                    Opened </span> :
                  item.statusId === "3118a07e-013a-4b3a-a2c1-74c921feeba1" ?
                  <span className='badge badge-light-success fs-8 fw-normal px-3 py-2 '>Accepted 
                  </span> :
                  item.statusId === "0dddbbcb-ac18-421a-942d-05ca579abb0c" ? 
                  <span className='badge badge-light-danger fs-8 fw-normal px-3 py-2 '>Rejected
                </span> :
                  item.statusId === "f49faffa-b113-4546-ac7f-485164e5a822" ? <span className='badge badge-light-info  fs-8 fw-normal px-3 py-2 '>
                  Closed </span> :
                  'Draft' } */}
                <AppStatusBadge value={item?.applicationStatus?.name} />
              </td>
              <td className="text-gray-600 fw-bold ">
                {item.moderator}
              </td>
              <td className="fw-bold ">{item?.comments}</td>
            </tr>
            // ({item.moderatorRole !== null ?  item.moderatorRole : ''})
          ))
        ) : (
          <tr>
            <td colSpan={4}></td>
          </tr>
        )}
      </tbody>
    </table>
  );
}
