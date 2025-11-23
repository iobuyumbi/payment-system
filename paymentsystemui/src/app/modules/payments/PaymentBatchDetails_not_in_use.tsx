import { useEffect, useState } from 'react'
import { PageLink, PageTitle } from '../../../_metronic/layout/core'

import { Outlet, Route, Routes } from 'react-router-dom'

import { useSelector } from 'react-redux'
import FarmerService from '../../../services/FarmerService'
import PaymentBatchHeader from './partials/PaymentBatchHeader'
import ListAssociateFarmers from './partials/ListAssociateFarmers'
import ListImportExcels from './partials/ListImportExcels'

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
        title: 'Payment Batch',
        path: '/payment-batch',
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

const PaymentBatchDetail_not_in_use = () => {

    const batch = useSelector((state: any) =>
        state?.paymentBatches,
    )

    useEffect(() => {
        document.title = "Payment batch Details - SDD";
    }, []);

    return (
        <Routes>
            <Route
                element={
                    <>
                        <PaymentBatchHeader
                            batch={batch}
                        />
                        <Outlet />
                    </>
                }
            >

                <Route
                    path='farmers'
                    element={
                        <>
                            <PageTitle breadcrumbs={breadCrumbs}>Payment Batch Details</PageTitle>
                            <ListAssociateFarmers id={batch.loanBatchId} CountryId={batch?.country?.id} batch={batch} />
                        </>
                    }
                />
                <Route
                    path='import-history'
                    element={
                        <>
                            <PageTitle breadcrumbs={breadCrumbs}>Payment Batch Details</PageTitle>
                            <ListImportExcels id={batch.loanBatchId} CountryId={batch?.country?.id} batch={batch} />
                        </>
                    }
                />
            </Route>

        </Routes>
    )
}

export default PaymentBatchDetail_not_in_use
