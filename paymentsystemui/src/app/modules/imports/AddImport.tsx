import { Content } from '../../../_metronic/layout/components/content';
import { KTCard, KTCardBody, KTIcon } from '../../../_metronic/helpers';
import { PageTitle } from '../../../_metronic/layout/core';
import { ToolbarWrapper } from '../../../_metronic/layout/components/toolbar';
import { Link } from 'react-router-dom';
import { useState } from 'react';
import { ImportModal } from '../../../_shared/Modals/ImportModal';


const AddImport = () => {
    const pageData = [
        {
            id: "_farmers",
            title: "Farmers",
            subTitle: "Onboard farmers quickly using Excel",
            module: "farmer",
            historyLink: "/imports/history"
        },
        // {
        //     id: "_loanappl",
        //     title: "Loan Applications",
        //     subTitle: "Upload loan applications quickly using Excel",
        //     module: "loanApplication",
        //     historyLink: "/imports/history"
        // },
        // {
        //     id: "_payment",
        //     title: "Payments ",
        //     subTitle: "Upload deductible/Facilitation Payments using Excel",
        //     module: "payments",
        //     historyLink: "/imports/history"
        // }
    ];

    const [showImport, setShowImport] = useState<boolean>(false);
    const [importBtnText, setImportBtnText] = useState<boolean>(false);
    const [exModule, setExModule] = useState<boolean>(false);

    const afterConfirm = (value: any) => {
        setShowImport(value);
    }

    const importClickHandler = (item: any) => {
        setImportBtnText(item.title);
        setExModule(item.module);

        setTimeout(() => {
            setShowImport(true);
        }, 500);
    }
    return (
        <>
            <PageTitle breadcrumbs={[]}>Imports</PageTitle>
            <ToolbarWrapper />
            <Content>
                <div className="bg-light-primary rounded p-9 mt-5">

                    <div className="d-flex w-75 my-3">
                        <KTIcon iconName='information' className='fs-2x me-5 text-primary' />
                        <p className='fs-5'>
                            Welcome to the Data Import feature!
                            This functionality allows you to easily upload and import data from Excel spreadsheets into our system.
                            Please follow the guidelines below to ensure a smooth import process.
                        </p>
                    </div>


                    <ul className='fs-5 mx-5' style={{ lineHeight: '2.0' }}>
                        <li><strong>Supported File Type</strong>: Please ensure your file is in Excel format (.xlsx or .xls). Other formats will not be accepted.</li>
                        <li><strong>Excel Template</strong>: Ensure that the data is filled as per the data structure in the pre-defined Excel template.</li>
                        <li><strong>Import History</strong>: Any failed records are reported in the Import History page. You may go through the errors reported there, make corrections
                            in data and the re-uload the template again.</li>
                    </ul>
                </div>
                <KTCard className={'card border my-3'}>
                    <KTCardBody>
                        {pageData.map((item: any, index: number) =>
                        (
                            <div key={item.id}>
                                <div className="d-flex align-items-center justify-content-between">
                                    <div className='w-50'>
                                        <h1 className='fs-2 text-gray-900 fw-normal'>{item.title}</h1>
                                        <span className='text-gray-500 fs-6'>{item.subTitle}</span>
                                    </div>

                                    <div>
                                        <button className='btn btn-theme' onClick={() => importClickHandler(item)}>Import Data</button>
                                    </div>
                                    <div>
                                        <Link to={item.historyLink} className='btn btn-secondary'>Import History</Link>
                                    </div>
                                </div>
                                {index < pageData.length - 1 && <div className="separator my-10"></div>}
                            </div>
                        ))}
                    </KTCardBody>
                </KTCard>
            </Content>
            {
                showImport && <ImportModal
                    title={importBtnText}
                    exModule={exModule}
                    afterConfirm={afterConfirm}
                />
            }
        </>
    )

}

export default AddImport;