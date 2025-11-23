import { Route, Routes, Outlet, Navigate } from 'react-router-dom'
import { PageLink, PageTitle } from '../../../_metronic/layout/core'
import { ImportFarmers } from './ImportFarmers'
import { ImportHistory } from './ImportHistory'


const rewardsBreadCrumbs: Array<PageLink> = [
    {
        title: 'Import',
        path: '/import',
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

const ImportPage = () => (
    <Routes>
        <Route element={<Outlet />}>
            <Route
                path='history'
                element={
                    <>
                        <PageTitle breadcrumbs={rewardsBreadCrumbs}>Import data</PageTitle>
                        <ImportHistory/>
                    </>
                }
            />
            <Route
                path='all'
                element={
                    <>
                        <PageTitle breadcrumbs={rewardsBreadCrumbs}>Import data</PageTitle>
                        <ImportFarmers/>
                    </>
                }
            />

            <Route index element={<Navigate to='history' />} />
        </Route>
    </Routes>
)

export default ImportPage
