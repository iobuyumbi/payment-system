import { useEffect, useState } from 'react'
import { PageLink, PageTitle } from '../../../_metronic/layout/core'

import { Outlet, Route, Routes } from 'react-router-dom'

import { useSelector } from 'react-redux'
import FarmerService from '../../../services/FarmerService'
import PaymentBatchHeader from './partials/PaymentBatchHeader'
import ListAssociateFarmers from './partials/ListAssociateFarmers'

const farmerService = new FarmerService();

const breadCrumbs: Array<PageLink> = [
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

const PaymentBatchDetail = () => {

    const batch = useSelector((state: any) =>
        state?.paymentBatches,
    )
   

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
                    path='associates'
                    element={
                        <>
                            <PageTitle breadcrumbs={breadCrumbs}>Payment Batch Details</PageTitle>
                            <ListAssociateFarmers id= {batch.loanBatchId} CountryId={batch?.country?.id} batch={batch}/>
                        </>
                    }
                />
                
               </Route>
        
        </Routes>
    )
}

export default PaymentBatchDetail
