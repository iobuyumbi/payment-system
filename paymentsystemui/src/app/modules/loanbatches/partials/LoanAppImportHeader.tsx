import { KTCard, KTCardBody } from '../../../../_metronic/helpers'
import LoanAppSuccessCard from './LoanAppSuccessCard'
import LoanAppWarningCard from './LoanAppWarningCard'


const LoanAppImportHeader = ({ data, importHistory, totalCounts }: any) => {
  return (
    <div>
      <KTCard className="mb-5">
        {/* begin::Header */}
        <div className="card-header">
          <h4 className="card-title align-items-start flex-column">
            Import History
          </h4>
        </div>
        {/* end::Header */}

        <KTCardBody>
          <div className="row">
            {/* <div className="col-md-5">
              <LoanAppInfoCard loanApp={data}/>
            </div> */}
            <div className="col-md-7">
              {data?.successRowCount > 0 &&
                <LoanAppSuccessCard batch={undefined} status={'Initiated'} isAllowed={function (permission: string): boolean {
                throw new Error('Function not implemented.')
              } } excelImportId={undefined} />
              }
              {/* {data?.successRowCount == 0 &&
                <LoanAppWarningCard />
              } */}
            </div>
          </div>
        </KTCardBody>
      </KTCard>
    </div>
  )
}

export default LoanAppImportHeader
