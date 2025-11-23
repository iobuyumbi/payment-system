import { KTCard, KTCardBody } from '../../../../_metronic/helpers'
import moment from 'moment';
import config from '../../../../environments/config';

const ImportTimeline = ({ importHistory, onItemClick }: any) => {
    return (
        <div>
            <KTCard>
                {/* begin::Header */}
                <div className='card-header' id='kt_help_header'>
                    <h4 className='card-title fw-bold fs-3'>Payment List Summary</h4>
                </div>
                {/* end::Header */}
                <KTCardBody>
                    {importHistory && importHistory.map((item: any, index: number) =>
                    (
                        <div key={index} onClick={() => onItemClick(item.excelImportId)}>
                            <div className="d-flex flex-column justify-content-justify fs-6">
                                <span className='text-gray-600 mb-3 '>{moment(item.importDate).format(config.dateOnlyFormat)}</span>
                                <div className="d-flex justify-content-between mb-3">
                                    <div className='text-gray-700'>Total Rows</div>
                                    <div className='text-gray-800 fw-bold fs-5'>{item.totalRowsInExcel}</div>
                                </div>
                                <div className="d-flex justify-content-between  mb-2">
                                    <div className='text-gray-700'>Passed Rows</div>
                                    <div className='text-gray-800'>{item.passedRows}</div>
                                </div>
                                <div className="d-flex justify-content-between  mb-2">
                                    <div className='text-gray-700'>Failed Rows</div>
                                    <div className='text-gray-800'>{item.failedRows}</div>
                                </div>
                            </div>
                            <div className="separator my-2"></div>
                        </div>
                    ))}

                </KTCardBody>
            </KTCard>
        </div>
    )
}

export default ImportTimeline