## Performance Testing Documentation – Social Media App



version: 1.0

1\. Test Overview



Application Name: Social Media App Version: 1.0 Deployment Environment: Docker container Database: MySQL (phpMyAdmin for management) Purpose of Test: Evaluate the system’s responsiveness, stability, and resource usage under typical and peak load conditions.

2\. Scope

Included



&nbsp;   API response time measurement



&nbsp;   Page load time measurement



&nbsp;   Database query performance



&nbsp;   Stress testing (high number of requests)



&nbsp;   Load testing (simulated concurrent users)



&nbsp;   Resource monitoring (CPU, RAM, Disk I/O)



2.1 Out of Scope



&nbsp;   Security stress testing



&nbsp;   Long‑term endurance testing (24h+)



&nbsp;   Network‑level packet analysis



3\. Test Environment

Host Systems



Arch Linux (rolling release)



&nbsp;   Kernel: 6.11.0‑arch1‑1



&nbsp;   Docker Version: not specified



Windows 11 Pro



&nbsp;   Build: 26100.7171



&nbsp;   Docker Desktop: 29.0.1



Containers

Service	Port

Frontend	3000

Backend API	5000

phpMyAdmin	8080

MySQL	3300

Tools Used



&nbsp;   Apache JMeter



&nbsp;   k6



&nbsp;   Docker stats



&nbsp;   Browser DevTools (Lighthouse)



4\. Performance Test Cases

ID	Test Case	Preconditions	Steps	Expected Result

P01	API Response Time	Backend running	Send 100 GET /posts requests	Avg response < 200ms

P02	Login Load Test	DB seeded	50 virtual users login simultaneously	No failures; response < 500ms

P03	Post Creation Stress Test	Logged‑in users	200 POST /posts requests in 1 min	System remains stable; no crashes

P04	DB Query Performance	DB active	Measure SELECT on posts table	Query < 50ms

P05	Frontend Load Time	Webapp running	Load homepage 10 times	Page loads < 2 seconds

P06	Resource Usage	All containers running	Monitor CPU/RAM during load	CPU < 80%, RAM < 70%

5\. Test Data



&nbsp;   50 test users



&nbsp;   200 sample posts



&nbsp;   500 comments



&nbsp;   1000 likes



6\. Expected Deliverables

Performance Report

ID	Result

P01	passed

P02	passed

P03	passed

P04	passed

P05	passed

P06	passed

Issues Found



&nbsp;   None detected



7\. Sign‑off



Tester: Suhajda Karina Date: 2025.11.29 Status: Completed

