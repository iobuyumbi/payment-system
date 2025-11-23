import { useEffect, useState } from "react";
import { KTIcon } from "../../../../_metronic/helpers";
import LoanApplicationService from "../../../../services/LoanApplicationService";
import moment from "moment";
const loanAppService = new LoanApplicationService();

const LoanAppList = (props: any) => {
  const { farmer } = props;

  const [data, setData] = useState<any>([]);
  const bindApplications = async () => {
    var result = await loanAppService.getFarmerLoanApplications(farmer?.id);
    if (result) {
      setData(result);
    } else {
      setData([])
    }
  };

  useEffect(() => {
    bindApplications();
  }, []);
  
  return (
    <div className="timeline-item">
      <div className="timeline-line w-40px"></div>

      {/* <div className='timeline-icon symbol symbol-circle symbol-40px'>
        <div className='symbol-label bg-light'>
          <KTIcon iconName='pencil' className='fs-2 text-gray-500' />
        </div>
      </div> */}

      <div className="timeline-content mb-10 mt-n1">
        {data &&
          data.map((item: any, index: number) => (
            <div className="overflow-auto pb-5" key={index}>
              <div className="notice d-flex bg-light-primary rounded border-primary border border-dashed min-w-lg-600px flex-shrink-0 p-6">
                <KTIcon
                  iconName="chart-simple-3coding/cod004.svg"
                  className="fs-2tx text-primary me-4"
                />
                <div className="d-flex flex-stack flex-grow-1 flex-wrap flex-md-nowrap">
                  <div className="mb-3 mb-md-0 fw-bold">
                    <h4 className="text-gray-800 fw-bolder">
                      {item.enumeratorFullName}
                    </h4>
                    <div className="fs-6 text-gray-700 pe-7">
                      Witness date :{" "}
                      {item.witnessDate
                        ? moment(item.witnessDate).format("DD-MMM-YYYY")
                        : ""}
                    </div>
                    <div className="fs-6 text-gray-700 pe-7">
                      Total seedlings: {item.totalSeedlings}
                    </div>
                    <div className="fs-6 text-gray-700 pe-7">
                      Grand total : {item.grandTotal}
                    </div>
                  </div>
                  <a
                    href="#"
                    className="btn btn-primary px-6 align-self-center text-nowrap"
                  >
                    View
                  </a>
                </div>
              </div>
            </div>
          ))}
      </div>
    </div>
  );
};

export { LoanAppList };
