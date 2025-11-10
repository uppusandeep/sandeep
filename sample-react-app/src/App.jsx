import { useEffect, useMemo, useState } from 'react'
import './App.css'

const API_BASE = import.meta.env.VITE_API_BASE ?? 'http://localhost:5299'

const initialClaimForm = {
  firstName: 'Jordan',
  lastName: 'Baker',
  dateOfBirth: '1993-07-15',
  email: 'jordan.baker@example.com',
  phoneNumber: '(859) 555-0198',
  county: 'Fayette',
  mailingAddress: '456 Vine Street, Lexington, KY 40507',
  employerName: 'Bluegrass Outdoors',
  startDate: '2022-03-12',
  lastDayWorked: '2025-02-14',
  separationReason: 'Seasonal layoff',
  averageWeeklyWage: '720',
  requestedWeeklyBenefitAmount: '0',
  preferredLanguage: 'English',
  preferredContactMethod: 'Email',
}

const initialCertificationForm = (weekEnding) => ({
  weekEnding,
  ableAndAvailable: true,
  activelySeekingWork: true,
  grossEarnings: '0',
  workSearchActivities: 'Applied via KCC, Reached out to former supervisor',
})

const numberFormatter = new Intl.NumberFormat('en-US', {
  style: 'currency',
  currency: 'USD',
  maximumFractionDigits: 2,
})

const enumFormatter = (value) =>
  typeof value === 'string'
    ? value.replace(/([A-Z])/g, ' $1').trim()
    : value

const dateFormatter = (value) => {
  if (!value) return '—'
  const parsed = new Date(value)
  if (Number.isNaN(parsed.getTime())) return value
  return parsed.toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  })
}

const computeNextWeekEnding = (claim) => {
  const base =
    claim?.weeklyCertifications?.length > 0
      ? claim.weeklyCertifications
          .map((c) => new Date(`${c.weekEnding}T00:00:00`))
          .sort((a, b) => b - a)[0]
      : new Date()

  const next = new Date(base)
  const day = next.getUTCDay()
  const delta = (6 - day + 7) % 7 || 7
  next.setUTCDate(next.getUTCDate() + delta)
  return next.toISOString().slice(0, 10)
}

const ClaimSummaryList = ({
  claims,
  selectedClaimId,
  onSelect,
  onRefresh,
  loading,
}) => (
  <section className="panel">
    <header className="panel-header">
      <div>
        <h2>Active Claims</h2>
        <p className="panel-subtitle">
          Kentucky unemployment insurance program overview
        </p>
      </div>
      <button type="button" className="link-button" onClick={onRefresh}>
        Refresh
      </button>
    </header>
    {loading ? (
      <p className="muted">Loading claims…</p>
    ) : claims.length === 0 ? (
      <p className="muted">No claims on file yet.</p>
    ) : (
      <ul className="claims-list">
        {claims.map((claim) => (
          <li key={claim.claimId}>
            <button
              type="button"
              className={`claim-tile ${
                selectedClaimId === claim.claimId ? 'selected' : ''
              }`}
              onClick={() => onSelect(claim.claimId)}
            >
              <div className="claim-header">
                <span className="claim-number">{claim.claimNumber}</span>
                <span className={`status-badge status-${claim.status}`}>
                  {enumFormatter(claim.status)}
                </span>
              </div>
              <div className="claim-body">
                <strong>{claim.claimantName}</strong>
                <span>Filed: {dateFormatter(claim.filedDate)}</span>
                <span>
                  Weekly benefit: {numberFormatter.format(claim.weeklyBenefitAmount)}
                </span>
              </div>
              <footer className="claim-footer">
                <span>{claim.weeksCertified} certifications</span>
                <span>{claim.weeksPaid} payments</span>
              </footer>
            </button>
          </li>
        ))}
      </ul>
    )}
  </section>
)

const ClaimDetails = ({
  claim,
  certificationForm,
  onCertificationChange,
  onCertificationSubmit,
  submitting,
}) => {
  if (!claim) {
    return (
      <section className="panel stretch">
        <header className="panel-header">
          <h2>Claim details</h2>
        </header>
        <p className="muted">Select a claim to see detailed information.</p>
      </section>
    )
  }

  const latestDetermination =
    claim.determinations?.length > 0
      ? claim.determinations.at(-1)
      : null

  return (
    <section className="panel stretch">
      <header className="panel-header">
        <div>
          <h2>Claim {claim.claimNumber}</h2>
          <p className="panel-subtitle">
            Status: <strong>{enumFormatter(claim.status)}</strong> · Benefit year
            ends {dateFormatter(claim.benefitYearEnd)}
          </p>
        </div>
        <div className="panel-meta">
          <span>
            Weekly benefit:{' '}
            <strong>{numberFormatter.format(claim.weeklyBenefitAmount)}</strong>
          </span>
          <span>Total certifications: {claim.weeklyCertifications?.length ?? 0}</span>
        </div>
      </header>

      <div className="grid two-col">
        <div className="card">
          <h3>Claimant</h3>
          <dl>
            <div>
              <dt>Name</dt>
              <dd>
                {claim.claimant.firstName} {claim.claimant.lastName}
              </dd>
            </div>
            <div>
              <dt>County</dt>
              <dd>{claim.claimant.county}</dd>
            </div>
            <div>
              <dt>Email</dt>
              <dd>{claim.claimant.email}</dd>
            </div>
            <div>
              <dt>Phone</dt>
              <dd>{claim.claimant.phoneNumber}</dd>
            </div>
            <div>
              <dt>Mailing address</dt>
              <dd>{claim.claimant.mailingAddress}</dd>
            </div>
          </dl>
        </div>

        <div className="card">
          <h3>Program flags</h3>
          <ul className="tag-list">
            {(claim.programFlags ?? []).map((flag) => (
              <li key={flag}>{flag}</li>
            ))}
          </ul>
          <h4>Most recent determination</h4>
          {latestDetermination ? (
            <div className="determination">
              <p>
                <strong>{latestDetermination.type}</strong> ·{' '}
                {enumFormatter(latestDetermination.outcome)}
              </p>
              <p className="muted">
                {dateFormatter(latestDetermination.issuedOn)} —{' '}
                {latestDetermination.summary}
              </p>
            </div>
          ) : (
            <p className="muted">No determinations yet.</p>
          )}
        </div>
      </div>

      <div className="card">
        <h3>Weekly certifications</h3>
        {claim.weeklyCertifications?.length ? (
          <div className="table">
            <div className="table-header">
              <span>Week ending</span>
              <span>Status</span>
              <span>Gross earnings</span>
              <span>Work search</span>
            </div>
            {claim.weeklyCertifications.map((cert) => (
              <div key={cert.certificationId} className="table-row">
                <span>{dateFormatter(cert.weekEnding)}</span>
                <span className={`status-badge status-${cert.status}`}>
                  {enumFormatter(cert.status)}
                </span>
                <span>{numberFormatter.format(cert.grossEarnings)}</span>
                <span>{cert.workSearchActivities.join(', ')}</span>
              </div>
            ))}
          </div>
        ) : (
          <p className="muted">No weekly certifications submitted yet.</p>
        )}
      </div>

      <div className="card">
        <h3>Payments</h3>
        {claim.payments?.length ? (
          <div className="table">
            <div className="table-header">
              <span>Issued</span>
              <span>Amount</span>
              <span>Method</span>
              <span>Status</span>
            </div>
            {claim.payments.map((payment) => (
              <div key={payment.paymentId} className="table-row">
                <span>{dateFormatter(payment.issueDate)}</span>
                <span>{numberFormatter.format(payment.amount)}</span>
                <span>{payment.method}</span>
                <span className={`status-badge status-${payment.status}`}>
                  {enumFormatter(payment.status)}
                </span>
              </div>
            ))}
          </div>
        ) : (
          <p className="muted">No benefit payments have been issued.</p>
        )}
      </div>

      <form className="card" onSubmit={onCertificationSubmit}>
        <h3>Submit weekly certification</h3>
        <div className="form-grid">
          <label>
            Week ending
            <input
              type="date"
              name="weekEnding"
              required
              value={certificationForm.weekEnding ?? ''}
              onChange={onCertificationChange}
            />
          </label>
          <label>
            Gross earnings
            <input
              type="number"
              min="0"
              step="0.01"
              name="grossEarnings"
              value={certificationForm.grossEarnings}
              onChange={onCertificationChange}
            />
          </label>
          <label className="checkbox">
            <input
              type="checkbox"
              name="ableAndAvailable"
              checked={certificationForm.ableAndAvailable}
              onChange={onCertificationChange}
            />
            Able and available to work this week
          </label>
          <label className="checkbox">
            <input
              type="checkbox"
              name="activelySeekingWork"
              checked={certificationForm.activelySeekingWork}
              onChange={onCertificationChange}
            />
            Completed required work search activities
          </label>
        </div>
        <label>
          Work search activities
          <textarea
            name="workSearchActivities"
            rows={3}
            value={certificationForm.workSearchActivities}
            onChange={onCertificationChange}
          />
          <span className="field-hint">
            Separate each activity with a comma (for example, “Applied via KCC”).
          </span>
        </label>
        <button type="submit" disabled={submitting}>
          {submitting ? 'Submitting…' : 'Submit certification'}
        </button>
      </form>
    </section>
  )
}

const ReferencePanel = ({ counties, workshops }) => (
  <aside className="panel">
    <header className="panel-header">
      <h2>Resources</h2>
      <p className="panel-subtitle">
        Kentucky Career Center programs and partner services
      </p>
    </header>
    <div className="card">
      <h3>Counties served</h3>
      <div className="county-grid">
        {counties.map((county) => (
          <span key={county} className="county-pill">
            {county}
          </span>
        ))}
      </div>
    </div>
    <div className="card">
      <h3>Upcoming workshops</h3>
      <ul className="workshop-list">
        {workshops.map((workshop) => (
          <li key={workshop.id}>
            <strong>{workshop.title}</strong>
            <span>{workshop.location}</span>
            <span>{workshop.county}</span>
            <span>
              {new Date(workshop.start).toLocaleString('en-US', {
                month: 'short',
                day: 'numeric',
                hour: 'numeric',
                minute: '2-digit',
              })}{' '}
              –{' '}
              {new Date(workshop.end).toLocaleTimeString('en-US', {
                hour: 'numeric',
                minute: '2-digit',
              })}
            </span>
          </li>
        ))}
      </ul>
    </div>
  </aside>
)

function App() {
  const [claims, setClaims] = useState([])
  const [selectedClaimId, setSelectedClaimId] = useState(null)
  const [selectedClaim, setSelectedClaim] = useState(null)
  const [counties, setCounties] = useState([])
  const [workshops, setWorkshops] = useState([])
  const [loadingClaims, setLoadingClaims] = useState(true)
  const [loadingDetails, setLoadingDetails] = useState(false)
  const [error, setError] = useState('')
  const [statusMessage, setStatusMessage] = useState('')
  const [claimForm, setClaimForm] = useState(initialClaimForm)
  const [certificationForm, setCertificationForm] = useState(
    initialCertificationForm(computeNextWeekEnding()),
  )
  const [submittingCertification, setSubmittingCertification] = useState(false)

  useEffect(() => {
    refreshClaims()
    fetchReferenceData()
  }, [])

  useEffect(() => {
    if (!selectedClaimId) {
      setSelectedClaim(null)
      return
    }
    fetchClaimDetails(selectedClaimId)
  }, [selectedClaimId])

  useEffect(() => {
    if (!selectedClaim) return
    setCertificationForm((current) => ({
      ...current,
      weekEnding: computeNextWeekEnding(selectedClaim),
    }))
  }, [selectedClaim?.claimId])

  const refreshClaims = async () => {
    try {
      setLoadingClaims(true)
      setError('')
      const response = await fetch(`${API_BASE}/api/claims`)
      if (!response.ok) {
        throw new Error('Unable to load claims from the Kentucky UI API.')
      }
      const data = await response.json()
      setClaims(data)
      setSelectedClaimId((current) => {
        if (current && data.some((claim) => claim.claimId === current)) {
          return current
        }
        return data.length > 0 ? data[0].claimId : null
      })
    } catch (err) {
      setError(err.message ?? 'Unexpected error while loading claims.')
    } finally {
      setLoadingClaims(false)
    }
  }

  const fetchClaimDetails = async (claimId) => {
    try {
      setLoadingDetails(true)
      setError('')
      const response = await fetch(`${API_BASE}/api/claims/${claimId}`)
      if (!response.ok) {
        throw new Error('Unable to load claim details.')
      }
      const data = await response.json()
      setSelectedClaim(data)
    } catch (err) {
      setError(err.message ?? 'Unexpected error while loading claim details.')
    } finally {
      setLoadingDetails(false)
    }
  }

  const fetchReferenceData = async () => {
    try {
      const [countiesResponse, workshopsResponse] = await Promise.all([
        fetch(`${API_BASE}/api/reference/counties`),
        fetch(`${API_BASE}/api/reference/workshops`),
      ])
      if (countiesResponse.ok) {
        setCounties(await countiesResponse.json())
      }
      if (workshopsResponse.ok) {
        setWorkshops(await workshopsResponse.json())
      }
    } catch (err) {
      console.warn('Unable to load reference data', err)
    }
  }

  const handleClaimFormChange = (event) => {
    const { name, value } = event.target
    setClaimForm((current) => ({
      ...current,
      [name]: value,
    }))
  }

  const handleCertificationFormChange = (event) => {
    const { name, type, value, checked } = event.target
    setCertificationForm((current) => ({
      ...current,
      [name]: type === 'checkbox' ? checked : value,
    }))
  }

  const handleCreateClaim = async (event) => {
    event.preventDefault()
    setError('')
    setStatusMessage('')

    const averageWeeklyWage = Number.parseFloat(
      claimForm.averageWeeklyWage || '0',
    )
    if (Number.isNaN(averageWeeklyWage) || averageWeeklyWage <= 0) {
      setError('Average weekly wage must be a positive number.')
      return
    }

    const requestedAmount =
      claimForm.requestedWeeklyBenefitAmount &&
      Number.parseFloat(claimForm.requestedWeeklyBenefitAmount) > 0
        ? Number.parseFloat(claimForm.requestedWeeklyBenefitAmount)
        : averageWeeklyWage * 0.45

    const payload = {
      claimant: {
        firstName: claimForm.firstName,
        lastName: claimForm.lastName,
        dateOfBirth: claimForm.dateOfBirth,
        email: claimForm.email,
        phoneNumber: claimForm.phoneNumber,
        county: claimForm.county,
        mailingAddress: claimForm.mailingAddress,
      },
      employmentHistory: {
        employerName: claimForm.employerName,
        startDate: claimForm.startDate,
        lastDayWorked: claimForm.lastDayWorked,
        separationReason: claimForm.separationReason,
      },
      averageWeeklyWage,
      requestedWeeklyBenefitAmount: requestedAmount,
      preferredLanguage: claimForm.preferredLanguage,
      preferredContactMethod: claimForm.preferredContactMethod,
    }

    try {
      const response = await fetch(`${API_BASE}/api/claims`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      })

      if (!response.ok) {
        throw new Error('Unable to create new unemployment claim.')
      }

      const createdClaim = await response.json()

      setClaims((current) => [
        {
          claimId: createdClaim.claimId,
          claimNumber: createdClaim.claimNumber,
          claimantName: `${createdClaim.claimant.firstName} ${createdClaim.claimant.lastName}`,
          filedDate: createdClaim.filedDate,
          benefitYearEnd: createdClaim.benefitYearEnd,
          status: createdClaim.status,
          weeklyBenefitAmount: createdClaim.weeklyBenefitAmount,
          weeksCertified: createdClaim.weeklyCertifications?.length ?? 0,
          weeksPaid:
            createdClaim.payments?.filter(
              (payment) => payment.status === 'Paid',
            ).length ?? 0,
        },
        ...current,
      ])
      setSelectedClaimId(createdClaim.claimId)
      setStatusMessage(
        `Claim ${createdClaim.claimNumber} filed for ${createdClaim.claimant.firstName} ${createdClaim.claimant.lastName}.`,
      )
      setClaimForm(initialClaimForm)
    } catch (err) {
      setError(err.message ?? 'Unexpected error creating claim.')
    }
  }

  const handleSubmitCertification = async (event) => {
    event.preventDefault()
    if (!selectedClaimId) {
      setError('Select a claim before submitting a weekly certification.')
      return
    }

    setError('')
    setStatusMessage('')
    setSubmittingCertification(true)

    const activities = certificationForm.workSearchActivities
      .split(',')
      .map((item) => item.trim())
      .filter(Boolean)

    const payload = {
      weekEnding: certificationForm.weekEnding,
      ableAndAvailable: certificationForm.ableAndAvailable,
      activelySeekingWork: certificationForm.activelySeekingWork,
      grossEarnings: Number.parseFloat(certificationForm.grossEarnings || '0'),
      workSearchActivities: activities.length ? activities : ['Not provided'],
    }

    try {
      const response = await fetch(
        `${API_BASE}/api/claims/${selectedClaimId}/weekly-certifications`,
        {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload),
        },
      )

      if (!response.ok) {
        throw new Error('Unable to submit weekly certification.')
      }

      await response.json()
      await refreshClaims()
      await fetchClaimDetails(selectedClaimId)
      setStatusMessage('Weekly certification submitted successfully.')
    } catch (err) {
      setError(err.message ?? 'Unexpected error submitting certification.')
    } finally {
      setSubmittingCertification(false)
    }
  }

  const headline = useMemo(() => {
    if (!selectedClaim) {
      return 'Kentucky Unemployment Insurance Portal'
    }
    return `${selectedClaim.claimant.firstName} ${selectedClaim.claimant.lastName} — Claim ${selectedClaim.claimNumber}`
  }, [selectedClaim])

  return (
    <div className="app-shell">
      <header className="app-header">
        <div className="header-inner">
          <div>
            <p className="eyebrow">Kentucky Career Center</p>
            <h1>{headline}</h1>
            <p className="lead">
              Track unemployment insurance claims, weekly certifications, and
              workforce services in one place.
            </p>
          </div>
          <div className="header-meta">
            <span>API base: {API_BASE}</span>
            <span>
              Active claims: <strong>{claims.length}</strong>
            </span>
          </div>
        </div>
      </header>

      {(error || statusMessage) && (
        <div className="banner-region">
          {error && <div className="banner error">{error}</div>}
          {statusMessage && <div className="banner success">{statusMessage}</div>}
        </div>
      )}

      <main className="content-grid">
        <ClaimSummaryList
          claims={claims}
          selectedClaimId={selectedClaimId}
          onSelect={setSelectedClaimId}
          onRefresh={refreshClaims}
          loading={loadingClaims}
        />

        {loadingDetails && selectedClaim ? (
          <section className="panel stretch">
            <header className="panel-header">
              <h2>Loading claim details…</h2>
            </header>
          </section>
        ) : (
          <ClaimDetails
            claim={selectedClaim}
            certificationForm={certificationForm}
            onCertificationChange={handleCertificationFormChange}
            onCertificationSubmit={handleSubmitCertification}
            submitting={submittingCertification}
          />
        )}

        <aside className="panel">
          <header className="panel-header">
            <h2>File a new claim</h2>
            <p className="panel-subtitle">
              Provide claimant and employment history to calculate benefits.
            </p>
          </header>

          <form className="card" onSubmit={handleCreateClaim}>
            <div className="form-grid">
              <label>
                First name
                <input
                  name="firstName"
                  value={claimForm.firstName}
                  onChange={handleClaimFormChange}
                  required
                />
              </label>
              <label>
                Last name
                <input
                  name="lastName"
                  value={claimForm.lastName}
                  onChange={handleClaimFormChange}
                  required
                />
              </label>
              <label>
                Date of birth
                <input
                  type="date"
                  name="dateOfBirth"
                  value={claimForm.dateOfBirth}
                  onChange={handleClaimFormChange}
                  required
                />
              </label>
              <label>
                County
                <select
                  name="county"
                  value={claimForm.county}
                  onChange={handleClaimFormChange}
                >
                  {counties.map((county) => (
                    <option key={county} value={county}>
                      {county}
                    </option>
                  ))}
                </select>
              </label>
              <label>
                Email
                <input
                  type="email"
                  name="email"
                  value={claimForm.email}
                  onChange={handleClaimFormChange}
                  required
                />
              </label>
              <label>
                Phone number
                <input
                  name="phoneNumber"
                  value={claimForm.phoneNumber}
                  onChange={handleClaimFormChange}
                />
              </label>
              <label className="full-width">
                Mailing address
                <input
                  name="mailingAddress"
                  value={claimForm.mailingAddress}
                  onChange={handleClaimFormChange}
                />
              </label>
            </div>

            <fieldset>
              <legend>Employment history</legend>
              <div className="form-grid">
                <label>
                  Employer name
                  <input
                    name="employerName"
                    value={claimForm.employerName}
                    onChange={handleClaimFormChange}
                    required
                  />
                </label>
                <label>
                  Start date
                  <input
                    type="date"
                    name="startDate"
                    value={claimForm.startDate}
                    onChange={handleClaimFormChange}
                    required
                  />
                </label>
                <label>
                  Last day worked
                  <input
                    type="date"
                    name="lastDayWorked"
                    value={claimForm.lastDayWorked}
                    onChange={handleClaimFormChange}
                    required
                  />
                </label>
              </div>
              <label>
                Separation reason
                <textarea
                  name="separationReason"
                  rows={3}
                  value={claimForm.separationReason}
                  onChange={handleClaimFormChange}
                />
              </label>
            </fieldset>

            <fieldset>
              <legend>Benefit calculation</legend>
              <div className="form-grid">
                <label>
                  Average weekly wage
                  <input
                    type="number"
                    min="0"
                    step="0.01"
                    name="averageWeeklyWage"
                    value={claimForm.averageWeeklyWage}
                    onChange={handleClaimFormChange}
                    required
                  />
                </label>
                <label>
                  Requested weekly benefit
                  <input
                    type="number"
                    min="0"
                    step="0.01"
                    name="requestedWeeklyBenefitAmount"
                    value={claimForm.requestedWeeklyBenefitAmount}
                    onChange={handleClaimFormChange}
                  />
                  <span className="field-hint">
                    Leave as 0 to use the automatic calculation (45% of wage, max
                    $650).
                  </span>
                </label>
              </div>
              <div className="form-grid">
                <label>
                  Preferred language
                  <select
                    name="preferredLanguage"
                    value={claimForm.preferredLanguage}
                    onChange={handleClaimFormChange}
                  >
                    <option>English</option>
                    <option>Spanish</option>
                    <option>Somali</option>
                    <option>Other</option>
                  </select>
                </label>
                <label>
                  Contact method
                  <select
                    name="preferredContactMethod"
                    value={claimForm.preferredContactMethod}
                    onChange={handleClaimFormChange}
                  >
                    <option>Email</option>
                    <option>Phone</option>
                    <option>Mail</option>
                  </select>
                </label>
              </div>
            </fieldset>

            <button type="submit">File unemployment claim</button>
          </form>
        </aside>
      </main>

      <ReferencePanel counties={counties} workshops={workshops} />
    </div>
  )
}

export default App
