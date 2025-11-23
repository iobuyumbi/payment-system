import ForceChangePassword from '../modules/auth/components/ForceChangePassword'
import { Content } from '../../_metronic/layout/components/content'
import { PageTitleWrapper } from '../../_metronic/layout/components/toolbar/page-title'
import { PageLink, PageTitle } from '../../_metronic/layout/core'
import { RequiredMarker } from '../../_shared/components';

const breadCrumbs: Array<PageLink> = [
    {
        title: 'Dashboard',
        path: '/dashboard',
        isSeparator: false,
        isActive: true,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: true,
    },
];

const ChangePassword = () => {
    return (
        <Content>
            <PageTitleWrapper />
            <PageTitle breadcrumbs={breadCrumbs}>Change Password</PageTitle>
            <div className="pt-2">
                <div className="card mb-5 mb-xl-10">
                    <div className="card-body p-9">
                        <RequiredMarker />
                        <div className="row">
                            <div className="col-md-6">
                                <ForceChangePassword />
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </Content>
    )
}

export default ChangePassword
