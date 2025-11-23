/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import { useEffect, useState } from "react";
import { Button, Col, Row } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import {FarmerInstructions } from "./_instructions";
import UploadLayout from "../../pages/UploadLayout";
import { PageTitle , PageLink} from "../../../_metronic/layout/core";

import axios from 'axios';

import { AUTH_LOCAL_STORAGE_KEY } from "../auth";
import { toast } from "react-toastify";
import { toastNotify } from "../../../services/NotifyService";

const axiosInstance = axios.create();
export function ImportFarmers(props: any) {
    const navigate = useNavigate()

    const [errors, setErrors] = useState<any>({});
    const [isvalid, setIsvalid] = useState<any>(false);
    const [uploaded, setUploaded] = useState<any>(false);
    const [files, setFiles] = useState<any>();
    const [exModule, setExModule] = useState<any>('');
    const [loading, setLoading] = useState(false)
    const [uploading, setUpLoading] = useState(false)
    const [orgId, setOrgId] = useState<any>('2BCA3C7F-237F-4677-82E7-238732F7975E');
    const [excelTypesArr, setExcelTypesArr] = useState<any>([])

    const redirect = () => {
        setTimeout(() => {
            navigate('/import/data/history')
        }, 500)
    }
    const baseURL ="https://localhost:44391/api"
   const uploadExcel=(files : any, url : any)=> {
        let token = '';
        const aware_user = JSON.parse(localStorage.getItem(AUTH_LOCAL_STORAGE_KEY)|| "");
        if (aware_user) {
            token = aware_user?.api_token;
        }
      
        const response = axiosInstance.post('/excelImport/importData', files,
            {
                baseURL: baseURL,
                headers: {
                    'Content-Type': 'multipart/form-data',
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'POST, GET, OPTIONS',
                    Authorization: 'Bearer ' + token,
                },
            });
    
        return response;
    }
    const profileBreadCrumbs: Array<PageLink> = [
        {
            title: 'Import Farmers',
            path: '/import/all',
            isSeparator: false,
            isActive: true,
        },
    ]
    const downLoadExcelTemplate = (values: any) => {
        // setLoading(true)
        // setTimeout(() => {
          

        //     downloadFarmer(values)
        //         .then((data : any) => {
        //             if (data) {
        //                 const url = window.URL.createObjectURL(new Blob([data.data]));
        //                 const link = document.createElement("a");
        //                 link.href = url;
        //                 link.setAttribute("download", `${(`${exModule}_${new Date().getDate()}_${new Date().getHours()}_${new Date().getMinutes()}`)}.xlsx`);
        //                 document.body.appendChild(link);
        //                 link.click();
        //             }
        //         })
        //         .catch(() => {

        //         })
        //         .finally(() => {
        //             setLoading(false)
        //         })
        // }, 1000)
    }

    const uploadFiles = (args: React.MouseEvent) => {
        args.preventDefault();
        // Clear error
        setErrors({});

        if (Object.keys(errors).length > 0) {
            setIsvalid(false);
        }

        if (files.length > 0) {
            const formData: FormData = new FormData();
            const moduleId = "4";

            for (let j = 0; j < files.length; j++) {
                formData.append("file", files[j], files[j].name);
                formData.append("orgId", orgId);
                formData.append("moduleId", moduleId);
            }
            setUpLoading(true);
            uploadExcel(formData, props.uploadUrl).then((response: any) => {
                const toastId = toast.loading("Please wait...");
                if (response) {
                    // alert(JSON.stringify(response));
                    if (response.data) {
                        redirect();
                        setUploaded(true);
                        
                        toastNotify(toastId, "Success");
                    } else {
                        errors.errorText = response.message;
                        toast.error(response.message);
                        setErrors(errors);
                    }
                } else {
                    errors.errorText = "There was an error. File could not be uploaded.";
                    toast.error(response.message);
                    setErrors(errors);
                }
                // after upload
            }).catch((e : any) => {
                console.log(e);
            })
                .finally(() => {
                    setUpLoading(false)
                });
        }
    };

    const bindExcelTypes = () => {
        const localArr: any[] = []

        //localArr.push({ id: 'categories', text: 'Categories' })
        localArr.push({ id: "Farmers", text: "Farmers details" });
        setExcelTypesArr(localArr);
      };
    

    const showMsgSection = (e: any) => {
        setExModule(e.target.value);
    }

    useEffect(() => {
        bindExcelTypes();
    }, []);

    useEffect(() => {
        // alert(exModule)
    }, [exModule]);

    return (
        <>
        <PageTitle breadcrumbs={profileBreadCrumbs}>Import Excel</PageTitle>

            <div className="d-flex justify-content-end align-items-end flex-wrap mb-5 ">
                <Link
                    to='/import/data/history'
                    className={`btn  btn-primary btn-active-light mx-3`}>
                    View Import History
                </Link>

            </div>

            <div className={`card card-xxl-stretch mb-xl-3`}>
                {/* begin::Header */}
                <div className='card-header border-bottom'>
                    <h3 className='card-title fw-bold text-dark'>Import Data</h3>
                </div>
                {/* end::Header */}
                {/* begin::Body */}
                <div className='card-body p-9'>
                    <Row className="mb-5 border-bottom">
                        <Col className="col-md-12 p-6">
                            <h4>Step 1:</h4>
                            <Col className="col-sm-12 fs-5 d-flex align-items-center py-3 ">
                                {" "}
                                <i className="bi bi-1-circle-fill fs-1 mx-3 text-gray-400"></i>
                                Select an Excel template to download.{" "}
                            </Col>
                            <div className="d-grid gap-2 my-3 w-50">
                                <select
                                    className={'form-control me-2'}
                                    onChange={(e) => showMsgSection(e)}
                                >
                                    <option key={0} value={0}>
                                        Select a template
                                    </option>
                                    {excelTypesArr &&
                                        excelTypesArr.map((sta: any) => (
                                            <option key={sta.id} value={sta.id}>
                                                {sta.text}
                                            </option>
                                        ))}
                                </select>
                            </div>

                            <div className="excel-upload bg-light-primary p-5">
                                <h1 className="fs-4">
                                    <i className="bi bi-1-circle-fill fs-4 mx-3"></i> Please read the instructions carefully before uploding excel.
                                </h1>
                               
                               
                                        <FarmerInstructions />
                              
                            </div>

                            <div className="d-grid gap-2 my-3 w-350px mt-5">
                                {
                                    (exModule === 'products') && (
                                        <Button variant="primary" onClick={downLoadExcelTemplate}>
                                            {!loading && <span className='indicator-label'><i className='bi bi-cloud-arrow-down-fill fs-3 py-2 mx-1'></i>
                                                Download template</span>}
                                            {loading && (
                                                <span className='indicator-progress' style={{ display: 'block' }}>
                                                    Please wait...
                                                    <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
                                                </span>
                                            )}

                                        </Button>
                                    )}
                            </div>
                        </Col>
                    </Row>
                   
                    <Row>
                        <Col className="col-md-6 p-6">
                            <h4>Step 2:</h4>
                            <Col className="col-sm-12 fs-5 d-flex align-items-center p-3">
                                {" "}
                                <i className="bi bi-3-circle-fill fs-1 mx-3 text-gray-400"></i>
                                Upload the completed file{" "}
                            </Col>
                            <UploadLayout setFiles={setFiles} setIsvalid={setIsvalid} ext={'xlsx'} />
                        {
                            !isvalid && files && files.length > 0 && <Col className="alert-error p-0">
                                {'Select only .xlsx file with 5MB limit.'}
                            </Col>
                        }
                            {/* {
                            files && files[0]?.fileName
                        } */}
                        </Col>
                    </Row>

                </div>
                <div className="card-footer">
                    <Button
                        variant="primary"
                        className="float-right"
                        disabled={!isvalid}
                        onClick={uploadFiles}
                    >
                        {!uploading && <span className='indicator-label'><i className='bi bi-cloud-arrow-up-fill fs-3 py-2 mx-1'></i>  Import </span>}
                        {uploading && (
                            <span className='indicator-progress' style={{ display: 'block' }}>
                                Please wait...
                                <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
                            </span>
                        )}

                    </Button>
                </div>
                {/* end::Body */}
            </div>
        </>
    )
}