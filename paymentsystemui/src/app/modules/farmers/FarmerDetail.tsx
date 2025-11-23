import { useEffect, useState } from 'react'
import { PageLink, PageTitle } from '../../../_metronic/layout/core'
import FarmerHeader from './partials/FarmerHeader'
import { Route, Routes } from 'react-router-dom'
import FarmerLoans from './partials/FarmerLoans'
import FarmerLoanApplications from './partials/FarmerLoanApplications'
import FarmerDocs from './partials/FarmerDocs'
import FarmerLocation from './partials/FarmerLocation'
import { useSelector } from 'react-redux'
import FarmerService from '../../../services/FarmerService'
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title'
import { FarmerLoanSchedule } from './partials/FarmerLoanSchedule'

const farmerService = new FarmerService();

const breadCrumbs: Array<PageLink> = [
    {
        title: 'Dashboard',
        path: '/dashboard',
        isSeparator: false,
        isActive: false,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: false,
    },
    {
        title: 'Farmers',
        path: '/farmers',
        isSeparator: false,
        isActive: false,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: false,
    },
]

const FarmerDetail = () => {

    const farmer = useSelector((state: any) =>
        state?.farmers,
    )
    const [coperatives, setCoperatives] = useState<any[]>([]);
    const [commaSeparated, setCommaSeparated] = useState<any>();
    const [farmerProjects, setFarmerProjects] = useState<any>();

    const getCooperatives = async () => {
        var response = await farmerService.getFarmerCooperatives(farmer.id);

        if (response) {
            setCoperatives(response);
            const labels = response.map(item => item.label).join(', ');
            setCommaSeparated(labels);
        }
    }

    const getFarmerProjects = async () => {
        var response = await farmerService.getFarmerProjects(farmer.id);

        if (response) {
            const labels = response.map(item => item.label).join(', ');
            setFarmerProjects(labels);
        }
    }

    useEffect(() => {
        document.title = `Farmer details - SDD`;
        getCooperatives();
        getFarmerProjects();
    }, []);

    return (
        <> 
          <PageTitle breadcrumbs={breadCrumbs}>Farmers</PageTitle>
        <Routes>
            <Route
                element={
                    <>
                        <FarmerHeader farmer={farmer} farmerProjects={farmerProjects} coperatives={commaSeparated} />
                        {/* <FarmerLoans farmerId={farmer.id} /> */}
                        {/* <Outlet /> */}
                    </>
                }
            >

                <Route
                    path='loans'
                    element={
                        <>
                            <PageTitle breadcrumbs={breadCrumbs}>Farmer Loans</PageTitle>
                            <FarmerLoans />
                        </>
                    }
                />
                <Route
                    path='loan-applications'
                    element={
                        <>
                            <PageTitle breadcrumbs={breadCrumbs}>Loan Applications</PageTitle>
                            <FarmerLoanApplications farmer={farmer} />
                        </>
                    }
                />
                  <Route
                    path='loan-schedule'
                    element={
                        <>
                            <PageTitle breadcrumbs={breadCrumbs}>Loan Schedule</PageTitle>
                            <FarmerLoanSchedule farmer={farmer} />
                        </>
                    }
                />
                <Route
                    path='documents'
                    element={
                        <>
                            <PageTitle breadcrumbs={breadCrumbs}>Documents</PageTitle>
                            <FarmerDocs />
                        </>
                    }
                />
                <Route
                    path='location'
                    element={
                        <>
                            <PageTitle breadcrumbs={breadCrumbs}>Loaction</PageTitle>
                            <FarmerLocation />
                        </>
                    }
                />

                {/* <Route index element={<Navigate to='/farmer-detail/overview' />} /> */}
            </Route>
        </Routes></>
    )
}

export default FarmerDetail
